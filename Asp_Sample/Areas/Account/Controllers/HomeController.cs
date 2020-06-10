using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Asp_Sample.Areas.Account.Models;
using DataLayer.Entity.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Asp_Sample.Areas.Account.Controllers
{
    [Area("Account")]
    public class HomeController : Controller
    {
        #region IOC

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        //private readonly IMessageSender _messageSender;

        public HomeController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager/*, IMessageSender messageSender*/)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            //_messageSender = messageSender;
        }

        #endregion

       // [Authorize]
        public IActionResult Index()
        {
            return View();
        }


        #region Register

        [Route("Account/Register")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [Route("Account/Register")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser()
                {
                    UserName = model.UserName,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    //var emailConfirmationToken =
                    //    await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var emailMessage =
                    //    Url.Action("ConfirmEmail", "Account",
                    //        new { username = user.UserName, token = emailConfirmationToken },
                    //        Request.Scheme);
                    //await _messageSender.SendEmailAsync(model.Email, "Email confirmation", emailMessage);

                    return RedirectToAction("Login");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            return View(model);
        }

        #endregion

        #region Login

        [Route("Account/Login")]
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            if (_signInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");

            var model = new LoginViewModel()
            {
                ReturnUrl = returnUrl,
                //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if(returnUrl != null)
                ViewData["returnUrl"] = returnUrl;
            return View(model);
        }

        [Route("Account/Login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (_signInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");

            model.ReturnUrl = returnUrl;
            //model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ViewData["returnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    model.UserName, model.Password, model.RememberMe, true);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);

                    return RedirectToAction("Index", "Home");
                }

                if (result.IsLockedOut)
                {
                    ViewData["ErrorMessage"] = "اکانت شما به دلیل پنج بار ورود ناموفق به مدت پنج دقیقه قفل شده است";
                    return View(model);
                }

                ModelState.AddModelError("", "رمزعبور یا نام کاربری اشتباه است");
            }
            return View(model);
        }

        #endregion

        #region LogOut

        [Route("Account/Logout")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Remote Validation

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return Json(true);
            return Json("ایمیل وارد شده از قبل موجود است");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsUserNameInUse(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null) return Json(true);
            return Json("نام کاربری وارد شده از قبل موجود است");
        }

        #endregion

        #region Email Confirmation

        //[HttpGet]
        //public async Task<IActionResult> ConfirmEmail(string userName, string token)
        //{
        //    if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(token))
        //        return NotFound();
        //    var user = await _userManager.FindByNameAsync(userName);
        //    if (user == null) return NotFound();
        //    var result = await _userManager.ConfirmEmailAsync(user, token);

        //    return Content(result.Succeeded ? "Email Confirmed" : "Email Not Confirmed");
        //}

        #endregion

        #region External Login

        //[HttpPost]
        //public IActionResult ExternalLogin(string provider, string returnUrl)
        //{
        //    var redirectUrl = Url.Action("ExternalLoginCallBack", "Account",
        //        new { ReturnUrl = returnUrl });

        //    var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        //    return new ChallengeResult(provider, properties);
        //}

        //public async Task<IActionResult> ExternalLoginCallBack(string returnUrl = null, string remoteError = null)
        //{
        //    returnUrl =
        //        (returnUrl != null && Url.IsLocalUrl(returnUrl)) ? returnUrl : Url.Content("~/");

        //    var loginViewModel = new LoginViewModel()
        //    {
        //        ReturnUrl = returnUrl,
        //        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
        //    };

        //    if (remoteError != null)
        //    {
        //        ModelState.AddModelError("", $"Error : {remoteError}");
        //        return View("Login", loginViewModel);
        //    }

        //    var externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
        //    if (externalLoginInfo == null)
        //    {
        //        ModelState.AddModelError("ErrorLoadingExternalLoginInfo", $"مشکلی پیش آمد");
        //        return View("Login", loginViewModel);
        //    }

        //    var signInResult = await _signInManager.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider,
        //        externalLoginInfo.ProviderKey, false, true);

        //    if (signInResult.Succeeded)
        //    {
        //        return Redirect(returnUrl);
        //    }

        //    var email = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);

        //    if (email != null)
        //    {
        //        var user = await _userManager.FindByEmailAsync(email);
        //        if (user == null)
        //        {
        //            var userName = email.Split('@')[0];
        //            user = new IdentityUser()
        //            {
        //                UserName = (userName.Length <= 10 ? userName : userName.Substring(0, 10)),
        //                Email = email,
        //                EmailConfirmed = true
        //            };

        //            await _userManager.CreateAsync(user);
        //        }

        //        await _userManager.AddLoginAsync(user, externalLoginInfo);
        //        await _signInManager.SignInAsync(user, false);

        //        return Redirect(returnUrl);
        //    }

        //    ViewBag.ErrorTitle = "لطفا با بخش پشتیبانی تماس بگیرید";
        //    ViewBag.ErrorMessage = $"دریافت کرد {externalLoginInfo.LoginProvider} نمیتوان اطلاعاتی از";
        //    return View();
        //}

        #endregion

    }
}