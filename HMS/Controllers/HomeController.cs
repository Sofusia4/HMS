using HMS.Interfaces;
using HMS.Models;
using HMS.Models.Pages;
using HMS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HMS.ViewModels;
using System.Diagnostics;
using System.Text;

namespace HMS.Controllers
{
	public class HomeController : Controller
	{
		private readonly IHotel _hotels;
		private readonly IRoom _rooms;
		//private readonly MapGenerator _generator;
		//private readonly IDeliveryInfo _deliveryInfo;



		private static string retUrl, pubId;

		public HomeController(IHotel hotels, IRoom rooms/*, MapGenerator generator, IDeliveryInfo deliveryInfo*/)
		{
			_hotels = hotels;
			_rooms = rooms;
			//_generator = generator;
			//_deliveryInfo = deliveryInfo;
		}

		public async Task<IActionResult> Index(QueryOptions? options, string? hotelId, RoomType[]? roomType, int[]? pricePerNight)
		{
			var allHotels = await _hotels.GetAllHotelsAsync();

			PagedList<Room> allRooms = _rooms.GetRoomsWithAdditionalOptions(hotelId, roomType, pricePerNight, options);

			//PagedList<Room> allRooms = _rooms.GetAll(options);

			//         if (hotelId != "all" && hotelId != null)
			//         {
			//	allRooms = _rooms.GetAllRoomsByHotel(options, hotelId);
			//}

			ViewBag.HOTELiD = hotelId;


			return View(new IndexViewModel
			{
				Hotels = allHotels.ToList(),
				Rooms = allRooms,
			});
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
