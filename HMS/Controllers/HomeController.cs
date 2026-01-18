using HMS.Interfaces;
using HMS.Models;
using HMS.Models.Pages;
using HMS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HMS.ViewModels;
using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace HMS.Controllers
{
	public class HomeController : Controller
	{
		private readonly IHotel _hotels;
		private readonly IRoom _rooms;
		private readonly IRoomOrder _orders;


		private static string retUrl, pubId;

		public HomeController(IHotel hotels, IRoom rooms, IRoomOrder orders)
		{
			_hotels = hotels;
			_rooms = rooms;
			_orders = orders;
		}

		public async Task<IActionResult> Index(QueryOptions? options, string? hotelId, RoomType[]? roomType, string? city, int capacity)
		{
			var allHotels = await _hotels.GetAllHotelsAsync();

			PagedList<Room> allRooms = _rooms.GetRoomsWithAdditionalOptions(hotelId, roomType, city, capacity, options);

			List<string> cities = await _hotels.GetAllCitiesAsync();
			int maxCapacity = (await _rooms.GetAllRoomsAsync()).Max(e => e.Capacity);
			
			ViewBag.HotelId = hotelId;
			ViewBag.Cities = cities;
			ViewBag.MaxCapacity = maxCapacity;
			ViewBag.RoomType = roomType;


			return View(new IndexViewModel
			{
				Hotels = allHotels.ToList(),
				Rooms = allRooms,
			});
		}

		[HttpGet]
		public async Task<IActionResult> FindRoomsForm()
		{
			List<string> cities = await _hotels.GetAllCitiesAsync();
			int maxCapacity = (await _rooms.GetAllRoomsAsync()).Max(e => e.Capacity);

			ViewBag.Cities = cities;
			ViewBag.MaxCapacity = maxCapacity;

			return View(new OrderViewModel());
		}

		[HttpPost]
		public async Task<IActionResult> FindRoomsForm(OrderViewModel model)
		{
			List<string> cities = await _hotels.GetAllCitiesAsync();
			int maxCapacity = (await _rooms.GetAllRoomsAsync()).Max(e => e.Capacity);

			ViewBag.Cities = cities;
			ViewBag.MaxCapacity = maxCapacity;

			if (ModelState.IsValid)
			{
				ViewBag.City = model.City;
				ViewBag.People = model.People;
				ViewBag.StartDate = model.StartDate;
				ViewBag.EndDate = model.EndDate;

				var rooms = await _rooms.GetAllRoomsWithHotelsAsync();
				var orders = await _orders.GetAllRoomOrdersWithRoomsAndUsersAsync();
				if (model.City != null && model.City != "all")
				{
					rooms = rooms.Where(e => e.Hotel.City.Contains(model.City) || e.Hotel.CityEng.Contains(model.City));
				}
				if (model.People > 0)
				{
					rooms = rooms.Where(e => e.Capacity >= model.People);
				}

				orders = orders.Where(e => (e.StartDate < model.StartDate && e.EndDate <= model.StartDate) || (e.StartDate >= model.EndDate && e.EndDate > model.EndDate));
				
				if (orders.Any())
				{
					var notAvailableRooms = orders.Select(e => e.RoomId).ToList();
					rooms = rooms.Where(e => !notAvailableRooms.Contains(e.Id.ToString()));
					if (rooms.Any())
					{
						ViewBag.Message = "";
						ViewBag.Rooms = rooms;
						return View(model);
					}
					else
					{
						ViewBag.Message = "Немає вільних кімнат";
						return View(model);
					}					
				}
				ViewBag.Message = "";
				ViewBag.Rooms = rooms;
				return View(model);

			}
			ViewBag.Message = "Немає вільних кімнат";
			return View(model);
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> MakeOrder([FromServices] UserManager<User> _userManager, string roomId, int people, DateTime startDate, DateTime endDate)
		{
			if (people > 0 && startDate != null && endDate != null)
			{
				int days = endDate.Subtract(startDate).Days;
				if (days > 0) 
				{
					Room room = await _rooms.GetRoomAsync(roomId);
					var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
					if (userId == null)
					{
						return NotFound();
					}
					var currentUser = await _userManager.FindByIdAsync(userId);

					if (room != null && currentUser != null)
					{
						RoomOrder order = new RoomOrder
						{
							StartDate = startDate,
							EndDate = endDate,
							PeopleCount = people,
							TotalPrice = days * room.PricePerNight,
							RoomId = roomId,
							Room = room,
							UserId = currentUser.Id,
							User = currentUser
						};
						await _orders.AddRoomOrderAsync(order);
					}
				}				
			}
			return RedirectToAction("Index");
		}

		[Authorize]
		[HttpGet]
		public async Task<IActionResult> MyOrders([FromServices] UserManager<User> _userManager, QueryOptions options)
		{
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return NotFound();
            }
            var currentUser = await _userManager.FindByIdAsync(userId);

			var orders = _orders.GetAllRoomOrdersByUser(options, currentUser.Id);

            return View(orders);
		}




		public async Task<IActionResult> GetRoom(/*[FromServices] IComment _comment, */string roomId, string? returnUrl)
		{
			if (roomId == null && pubId != null)
			{
				roomId = pubId;
			}
			if (returnUrl == null && retUrl != null)
			{
				returnUrl = retUrl;
			}

			Room room = await _rooms.GetRoomAsync(roomId);
			if (room != null)
			{
				return View(new RoomViewModel()
				{
					Room = room,
					ReturnUrl = returnUrl//,

					//Comments = _comment.GetCommentsByProductId(product.Id.ToString()).ToList()
				});
			}
			return NotFound();
		}

		[Route("get-more-rooms")]
		[HttpGet]
		public async Task<IActionResult> GetMoreRooms(int page = 1, int pageSize = 10)
		{
			return PartialView("_RoomsPartial", await _rooms.GetRoomWithPageAsync(page, pageSize));
		}



		//[Route("/sitemap.xml")]
		//public ActionResult SitemapXml()
		//{
		//	var result = _generator.GetSitemapNodes(this.Url);
		//	string host = Request.Scheme + "://" + Request.Host;

		//	var sitemapNodes = _generator.GetSitemapNodes(this.Url);
		//	string xml = _generator.GetSitemapDocument(sitemapNodes);
		//	return this.Content(xml, "text/xml", Encoding.UTF8);
		//}

		//#region Comments
		//[HttpPost]
		//[AllowAnonymous]
		//public async Task<IActionResult> AddComment([FromServices] IComment _comment, Comment comment)
		//{
		//	if (comment != null)
		//	{
		//		if (!Captcha.ValidateCaptchaCode(comment.CaptchaCode, HttpContext))
		//		{
		//			ModelState.AddModelError("CaptchaCode", "Invalid captcha");
		//			pubId = comment.IdProduct.ToString();
		//			retUrl = comment.ReturnUrl;
		//			return RedirectToAction(actionName: nameof(GetProduct)); ;
		//		}

		//		if (comment.Name.IsNullOrEmpty())
		//		{
		//			comment.Name = "Unknown visitor";
		//		}
		//		comment.CreatedAt = DateTime.Now;
		//		await _comment.AddCommentAsync(comment);
		//	}
		//	pubId = comment.IdProduct.ToString();
		//	retUrl = comment.ReturnUrl;
		//	return RedirectToAction(actionName: nameof(GetProduct));
		//}

		//public IActionResult GetCaptchaImage()
		//{
		//	int width = 200;
		//	int height = 72;
		//	var captchaCode = Captcha.GenerateCaptchaCode();
		//	var result = Captcha.GenerateCaptchaImage(width, height, captchaCode);
		//	HttpContext.Session.SetString("CaptchaCode", result.CaptchaCode);
		//	Stream s = new MemoryStream(result.CaptchaByteData);
		//	return new FileStreamResult(s, "image/png");
		//}


		//#endregion


		//#region Delivery Information
		//[Route("/delivery")]
		//public async Task<IActionResult> DeliveryInformation()
		//{
		//	DeliveryInfo info = await _deliveryInfo.GetDeliveryInfo();
		//	return View(info);
		//}
		//#endregion

		//public IActionResult Privacy()
		//{
		//	return View();
		//}

		//[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		//public IActionResult Error()
		//{
		//	return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		//}
	}
}
