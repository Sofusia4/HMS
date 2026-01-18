using HMS.Interfaces;
using HMS.Models.Pages;
using HMS.Models;
using HMS.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using MailKit.Search;

namespace HMS.Controllers
{
	[Authorize(Roles = "Admin,Editor")]
	public class OrderController : Controller
	{
		private readonly IRoomOrder _order;

		public OrderController(IRoomOrder order)
		{
			_order = order;
		}

		public IActionResult Orders(QueryOptions options)
		{
			return View(_order.GetAll(options));
		}


		[Route("order/delete-order")]
		[HttpDelete]
		public async Task<IActionResult> DeleteOrder(string orderId)
		{
			var order = await _order.GetRoomOrderAsync(orderId);
			if (order != null)
			{
				await _order.DeleteRoomOrderAsync(order);
			}
			else
			{
				return NotFound();
			}

			return Ok();
		}


		[Route("order/create-update-order")]
		[HttpGet]
		public async Task<IActionResult> CreateOrUpdateOrder(string orderId)
		{
			CreateOrUpdateOrderViewModel model;

			if (orderId != null)
			{				
				RoomOrder order = await _order.GetRoomOrderAsync(orderId);
				if (order == null)
				{
					return NotFound();
				}
				model = new CreateOrUpdateOrderViewModel()
				{
					Id = orderId,
                    People = order.PeopleCount,
					StartDate = order.StartDate,
					EndDate = order.EndDate,
					TotalPrice = order.TotalPrice,
					RoomId = order.RoomId,
					UserId = order.UserId
				};
								
				return View(model);
			}
			model = new CreateOrUpdateOrderViewModel()
			{
				Id = "create",
                People = 0,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(1),
                TotalPrice = 0,
                RoomId = "",
                UserId = ""
			};

			return View(model);
		}

		[Route("order/create-update-order")]
		[HttpPost]
		public async Task<IActionResult> CreateOrUpdateOrder([FromServices] IRoom _rooms, [FromServices] UserManager<User> _userManager, CreateOrUpdateOrderViewModel model)
		{
			if (ModelState.IsValid)
			{
				if (model.Id == "create")
				{
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (userId == null)
                    {
                        return NotFound();
                    }
                    var currentUser = await _userManager.FindByIdAsync(userId);


                    Room room = await _rooms.GetRoomAsync(model.RoomId);

                    RoomOrder order = new RoomOrder()
					{
                        PeopleCount = model.People,
                        StartDate = model.StartDate,
                        EndDate = model.EndDate,
                        TotalPrice = model.TotalPrice,
                        RoomId = model.RoomId,
						Room = room,
                        UserId = model.UserId,
						User = currentUser
					};

					await _order.AddRoomOrderAsync(order);

					return RedirectToAction(nameof(Orders));
				}
				else
				{
					RoomOrder order = await _order.GetRoomOrderAsync(model.Id);
					if (order == null)
					{
						return NotFound();
					}

                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (userId == null)
                    {
                        return NotFound();
                    }
                    var currentUser = await _userManager.FindByIdAsync(userId);

                    Room room = await _rooms.GetRoomAsync(model.RoomId);

                    order.PeopleCount = model.People;
					order.StartDate = model.StartDate;
					order.EndDate = model.EndDate;
					order.TotalPrice = model.TotalPrice;
					order.RoomId = model.RoomId;
					order.Room = room;
					order.UserId = userId;
					order.User = currentUser;


					await _order.UpdateRoomOrderAsync(order);

					return RedirectToAction(nameof(Orders));
				}
			}
			return View(model);
		}
	}
}
