using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using HMS.Interfaces;
using HMS.Models;
using HMS.Models.Pages;
using HMS.ViewModels;
using System.Security.Claims;

namespace HMS.Controllers
{
    [Authorize(Roles="Admin, Editor")]
    public class PanelController : Controller
    {
		private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        //private readonly IContactInfo _contactInfo;
        //private readonly IDeliveryInfo _deliveryInfo;
		private readonly RoleManager<IdentityRole> _roleManager;

		public PanelController(UserManager<User> userManager, SignInManager<User> signInManager, /*IContactInfo contactInfo, IDeliveryInfo deliveryInfo,*/ RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			//_contactInfo = contactInfo;
			//_deliveryInfo = deliveryInfo;
			_roleManager = roleManager;
		}

		public IActionResult Index()
        {
            return View();
        }

        #region users

        [Authorize(Roles="Admin")]
        public IActionResult Users(QueryOptions options)
        {
            PagedList<User> users = new PagedList<User>(_userManager.Users, options);
            return View(users);
		}

        [Authorize(Roles = "Admin")]
        [Route("panel/delete-user")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var userId2 = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId2 == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (userId2 == userId)
            {
                await _signInManager.SignOutAsync();
                await _userManager.DeleteAsync(user);
                return RedirectToAction(actionName: nameof(Index), controllerName: "Home");
            }
            await _userManager.DeleteAsync(user);

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [Route("create-update-user")]
        [HttpGet]
        public async Task<IActionResult> CreateOrUpdateUser(string userId)
        {
            CreateOrUpdateUserViewModel model;

            if (userId != null)
            {
                User user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound();
                }
                model = new CreateOrUpdateUserViewModel()
                {
                    Id = userId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                };
                return View(model);
            }
            model = new CreateOrUpdateUserViewModel()
            {
                Id = "create",
                FirstName = "",
                LastName = "",
                Email = "",
                PhoneNumber = ""
            };

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [Route("create-update-user")]
        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateUser(CreateOrUpdateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id == "create")
                {
                    User user = new User()
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        UserName = model.Email,
                        PhoneNumber = model.PhoneNumber
                    };

                    var result = await _userManager.CreateAsync(user);
                    if (!result.Succeeded)
                    {
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                        return View(model);
                    }

                    await _userManager.AddPasswordAsync(user, model.Password);
                    if (!result.Succeeded)
                    {
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                        return View(model);
                    }

                    user = await _userManager.FindByEmailAsync(user.Email);
                    await _userManager.AddToRoleAsync(user, "Guest");

                    return RedirectToAction(nameof(Users));
                }
                else
                {
                    User user = await _userManager.FindByIdAsync(model.Id);
                    if (user == null)
                    {
                        return NotFound();
                    }
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Email = model.Email;
                    user.PhoneNumber = model.PhoneNumber;

                    var result = await _userManager.UpdateAsync(user);
                    if (!result.Succeeded)
                    {
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                    }
                    else
                    {
                        return RedirectToAction(nameof(Users));
                    }
                }
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [Route("user-roles")]
        [HttpGet]
        public async Task<IActionResult> EditRoles(string userId)
        {
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                ChangeRoleViewModel model = new ChangeRoleViewModel
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return View(model);
            }
            return NotFound();
        }

        [Authorize(Roles = "Admin")]
        [Route("user-roles")]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> EditRoles(string userId, List<string> roles)
        {
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var addedRoles = roles.Except(userRoles);
                var removedRoles = userRoles.Except(roles);

                await _userManager.AddToRolesAsync(user, addedRoles);
                await _userManager.RemoveFromRolesAsync(user, removedRoles);

                return RedirectToAction(nameof(Users));
            }
            return NotFound();
        }


        #endregion

  //      #region contactInfo

  //      public IActionResult Contact()
  //      {
  //          ContactInfo info = _contactInfo.GetContactInfo();
  //          return View(info);
  //      }

  //      [HttpGet]
  //      [Route("update-contact")]
  //      public IActionResult UpdateContact()
  //      {
		//	ContactInfo info = _contactInfo.GetContactInfo();
  //          if (info == null)
  //          {
  //              info = new ContactInfo() 
  //              { 
  //                  Id = 0,
		//			Email = "pizzaking@gmail.com",
		//			PhoneNumber = "+380961234567",
		//			Address = "Khreshchatyk street, Kyiv, Ukraine",
		//			Instagram = "https://www.instagram.com/pizzaking_ukraine/",
		//			Facebook = "https://www.facebook.com/pizzaking.ukraine/?locale=ru_RU"
		//		};
  //          }
		//	return View(info);
		//}

		//[HttpPost]
		//[Route("update-contact")]
		//public IActionResult UpdateContact(ContactInfo info)
		//{
  //          if (ModelState.IsValid)
  //          {
  //              _contactInfo.Update(info);
		//	}
			
		//	return View(info);
		//}


		//#endregion

		//#region DeliveryInfo
		//[HttpGet]
		//[Route("update-delivery-info")]
		//public async Task<IActionResult> UpdateDeliveryInfo()
		//{
		//	DeliveryInfo info = await _deliveryInfo.GetDeliveryInfo();
		//	if (info == null)
		//	{
		//		info = new DeliveryInfo()
		//		{
		//			Id = 0,
		//			Email = "pizzaking@gmail.com",
		//			PhoneNumber = "+380961234567",
		//			Address = "Khreshchatyk street, Kyiv, Ukraine",
  //                  Text = "<h2 class=\"title\">What is the Pizza Tracker service: an overview</h2>\r\n<p class=\"description\">\r\n\tGood service is what sets a pizzeria apart from other establishments. \r\n\tYou can order pizza in advance by specifying a convenient time for delivery. \r\n\tIf you need an order urgently, the PizzaKing team will take up to 30 minutes to fulfill it.\r\n</p>\r\n<p class=\"description\">\r\n\tIn order not to wait for the courier for an extra time, it is better to order in advance. \r\n\tYou can choose everything you need for the order in a timely manner and determine the time when the courier should arrive.\r\n</p>\r\n<h2 class=\"title\">Why is the delivery so fast?</h2>\r\n<p class=\"description\">\r\n\tThe speed of delivery is often influenced by the cooking time of the pizza. \r\n\tIt usually takes from 8 to 20 minutes, depending on the thickness of the dough, \r\n\tthe amount of filling and the standard of moisture of the products in the finished dish. \r\n\tPizza is prepared in the oven at a temperature of 180 to 200°C.\r\n</p>\r\n<p class=\"description\">\r\n\tFor example, for thin dough, semi-finished products are often used - then the filling \r\n\tand the cake itself are baked in the oven at the same time. \r\n\tFor a thick dough, choose wet ingredients - then the base will not dry out, \r\n\tand the filling will be prepared in a timely manner.\r\n</p>\r\n<p class=\"description\">\r\n\tEntrust your gastronomic delight to pizzaiolo at PizzaKing. \r\n\tHere, every recipe is thought out to the smallest detail — from the amount of filling to the cooking time. \r\n\tAs a result, the meat is baked, but not dried out, the vegetables remain perfectly juicy, \r\n\tand the cheese melts and spreads in a viscous layer. \r\n\tIn addition, the chefs and pizzaiolo use only the best ingredients for cooking. \r\n\tAll products are checked for suitability before being sent to the letter.\r\n</p>"
		//		};
		//	}
		//	return View(info);
		//}

		//[HttpPost]
		//[Route("update-delivery-info")]
		//public IActionResult UpdateDeliveryInfo(DeliveryInfo info)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		_deliveryInfo.Update(info);
		//	}

		//	return View(info);
		//}
		//#endregion

		public IActionResult Dishes()
        {
            return View();
        }
        public IActionResult Categories()
        {
            return View();
        }        
        public IActionResult Statistics()
        {
            return View();
        }
    }
}
