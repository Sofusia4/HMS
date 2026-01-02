using HMS.Helpers;
using HMS.Interfaces;
using HMS.Models;
using HMS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace HMS.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
        private readonly EmailConfiguration _emailConfig;
        private readonly IUserDelete _delete;


        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, EmailConfiguration emailConfig, IUserDelete delete)
		{
			_userManager = userManager;
			_signInManager = signInManager;
            _emailConfig = emailConfig;
            _delete = delete;

        }

        #region Login
        [Route("login")]
		[HttpGet]
		public IActionResult Login(string returnUrl = null)
		{
			return View(new LoginViewModel { ReturnUrl = returnUrl });
		}

		[Route("login")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				var result =
					await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
				if (result.Succeeded)
				{
					if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
					{
						return Redirect(model.ReturnUrl);
					}
					else
					{
						return RedirectToAction("Index", "Home");
					}
				}
				else
				{
					ModelState.AddModelError("", "Wrong login and (or) password");
				}
			}
			return View(model);
		}
        #endregion

        #region Logout
        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}
        #endregion

        #region Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Email, FirstName = model.FirstName, LastName = model.LastName, PhoneNumber = model.PhoneNumber };

                if (!string.IsNullOrEmpty(model.Email))
                {
                    var link = Url.Action("CheckEmail", "Account", new { email = model.Email, password = model.Password, confirmed = model.PasswordConfirm, firstName = model.FirstName, lastName = model.LastName, phoneNumber = model.PhoneNumber }, Request.Scheme);

                    EmailHelper emailHelper = new EmailHelper(_emailConfig);
                    bool emailResponse = emailHelper.SendEmailCheck(user, link);

                    if (emailResponse)
                        return RedirectToAction("EmailConfirmation", "Account");
                    else
                    {
                        // log email failed 
                        ModelState.AddModelError("", "An error occurred, please try again later.");
                    }

                }
            }
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> CheckEmail(string email, string password, string confirmed, string firstName, string lastName, string phoneNumber)
        {
            RegisterViewModel model = new RegisterViewModel() { Email = email, Password = password, PasswordConfirm = confirmed, FirstName = firstName, LastName = lastName, PhoneNumber = phoneNumber };
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Email, FirstName = firstName, LastName = lastName, PhoneNumber = phoneNumber };


                // добавляем пользователя
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // установка куки
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return RedirectToAction(actionName: nameof(Register), routeValues: model);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult EmailConfirmation()
        {
            return View();
        }
        #endregion

        #region ForgotPassword

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword([Required] string email)
        {
            if (email == null)
                return View(model: email);

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return RedirectToAction(nameof(ForgotPassword));

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var link = Url.Action("ResetPassword", "Account", new { token, email = user.Email }, Request.Scheme);

            EmailHelper emailHelper = new EmailHelper(_emailConfig);
            bool emailResponse = emailHelper.SendEmailPasswordReset(user, link);

            if (emailResponse)
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            else
            {
                // log email failed 
                ModelState.AddModelError("", "An error occurred, please try again later.");
            }
            return View(model: email);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }


        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPassword { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPassword resetPassword)
        {
            if (!ModelState.IsValid)
                return View(resetPassword);

            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user == null)
                RedirectToAction(nameof(ResetPasswordConfirmation));

            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);
            if (!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                    ModelState.AddModelError(error.Code, error.Description);
                return View(resetPassword);
            }

            return RedirectToAction(nameof(ResetPasswordConfirmation));
        }

        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        #endregion

        #region UserSettings
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> AccountSettings()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return NotFound();
            }
            var currentUser = await _userManager.FindByIdAsync(userId);
            UserSettingsViewModel usvm = new UserSettingsViewModel()
            {
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                PhoneNumber = currentUser.PhoneNumber,
                Email = currentUser.Email
            };
            return View(usvm);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AccountSettings(UserSettingsViewModel usvm)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    return NotFound();
                }
                var currentUser = await _userManager.FindByIdAsync(userId);

                if (currentUser != null)
                {
                    if (!string.IsNullOrEmpty(usvm.Email))
                    {
                        var token = await _userManager.GenerateChangeEmailTokenAsync(currentUser, usvm.Email);
                        await _userManager.ChangeEmailAsync(currentUser, usvm.Email, token);
                    }
                    if (!string.IsNullOrEmpty(usvm.FirstName))
                    {
                        currentUser.FirstName = usvm.LastName;
                        var s = await _userManager.UpdateAsync(currentUser);
                    }
                    if (!string.IsNullOrEmpty(usvm.LastName))
                    {
                        currentUser.LastName = usvm.LastName;
                        var s = await _userManager.UpdateAsync(currentUser);
                    }
                    if (!string.IsNullOrEmpty(usvm.PhoneNumber))
                    {
                        currentUser.PhoneNumber = usvm.PhoneNumber;
                        var s = await _userManager.UpdateAsync(currentUser);
                    }
                }
            }

            var userId2 = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId2 == null)
            {
                return NotFound();
            }
            var currentUser2 = await _userManager.FindByIdAsync(userId2);
            UserSettingsViewModel usvm2 = new UserSettingsViewModel()
            {
                FirstName = currentUser2.FirstName,
                LastName = currentUser2.LastName,
                PhoneNumber = currentUser2.PhoneNumber,
                Email = currentUser2.Email
            };
            return View(usvm2);
        }

        #endregion

        #region DeleteAccount
        [Authorize]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeleteAccount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return NotFound();
            }
            var currentUser = await _userManager.FindByIdAsync(userId);
            await _signInManager.SignOutAsync();

            UserDelete userDelete = new UserDelete()
            {
                UserId = userId,
                DeletedAt = DateTime.Now
            };
            await _delete.CreateAsync(userDelete);

            return RedirectToAction(actionName: nameof(Index), controllerName: "Home");
        }
        #endregion

    }
}
