using DemoMVC01.Areas.Identity.ViewModels.Account;
using DemoMVC01.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace DemoMVC01.Areas.Identity.Controllers
{
    
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IEmailSender emailSender,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        // GET: /Account/Login
        [HttpGet("/login/")]
        [AllowAnonymous]
        public async Task<dynamic> Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            IList<AuthenticationScheme> externalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            LoginViewModel model = new();
            model.ExternalLogins = externalLogins;
            return View(model);
        }

        //
        // POST: /Account/Login
        [HttpPost("/login/")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Username,Name,Password,RememberMe")]  LoginViewModel model, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ViewData["ReturnUrl"] = returnUrl;
        
            if (ModelState.IsValid)
            {

                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);
                // Tìm UserName theo Email, đăng nhập lại
                /*if ((!result.Succeeded) && AppUtilities.IsValidEmail(model.UserNameOrEmail))
                {
                    var user = await _userManager.FindByEmailAsync(model.UserNameOrEmail);
                    if (user != null)
                    {
                        result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: true);
                    }
                }*/

                if (result.Succeeded)
                {
                    _logger.LogInformation(1, "Login thanh cong");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    //return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                }

                if (result.IsLockedOut)
                {
                    _logger.LogWarning(2, "Tài khoản bị khóa");
                    return View("Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Không đăng nhập được.");
                    model.ExternalLogins= (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
                    return View(model);
                }
            }
            return View(model);
        }

        // POST: /Account/LogOff
        [HttpPost("/logout/")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(string returnUrl=null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User đăng xuất");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }
        //
        // GET: /Account/Register
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Register(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ViewData["ReturnUrl"] = returnUrl;
            IList<AuthenticationScheme> externalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            RegisterViewModel model = new();
            model.ExternalLogins = externalLogins;
            return View(model);
        }
        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            returnUrl = Url.Content("~/");
            
            ViewData["ReturnUrl"] = returnUrl; 
            if (ModelState.IsValid)
            {
                var user = new AppUser { UserName = model.Username, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Đã tạo user mới.");

                    // Phát sinh token để xác nhận email
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    // https://localhost:5001/confirm-email?userId=fdsfds&code=xyz&returnUrl=
                    var callbackUrl = Url.ActionLink(
                        action: nameof(ConfirmEmail),
                        values:
                            new
                            {
                                area = "Identity",
                                userId = user.Id,
                                code = code
                            },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(model.Email,
                        "Xác nhận địa chỉ email",
                        @$"Bạn đã đăng ký tài khoản trên RazorWeb, 
                           hãy <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>bấm vào đây</a> 
                           để kích hoạt tài khoản.");
                    
                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return LocalRedirect(Url.Action(nameof(RegisterConfirmation)));
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }

                }
                
                foreach (var item in result.Errors)
                {
                    if (item.Code.Equals("DuplicateUsername"))
                    {
                        //ModelState.AddModelError("Username", "Username da duoc su dung");
                    }
                    if (item.Code.Equals("DuplicateEmail"))
                    {
                        ModelState.AddModelError("Email", "Email da duoc su dung");
                    }
                }
                
            }

            // If we got this far, something failed, redisplay form
            model.ExternalLogins= (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            return View(model);
        }
        // GET: /Account/ConfirmEmail
        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegisterConfirmation()
        {
            return View();
        }
        // GET: /Account/ConfirmEmail
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("ErrorConfirmEmail");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("ErrorConfirmEmail");
            }
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "ErrorConfirmEmail");
        }
        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }
        //
        // GET: /Account/ExternalLoginCallback
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl ??= Url.Content("~/");
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Lỗi sử dụng dịch vụ ngoài: {remoteError}");
                return View(nameof(Login));
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState.AddModelError(string.Empty, $"Error loading external login information.");
                return View(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (result.Succeeded)
            {
                // Cập nhật lại token
                await _signInManager.UpdateExternalAuthenticationTokensAsync(info);

                _logger.LogInformation(5, "User logged in with {Name} provider.", info.LoginProvider);
                return LocalRedirect(returnUrl);
            }
            if (result.RequiresTwoFactor)
            {
                //return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl });
            }
            if (result.IsLockedOut)
            {
                return View("Lockout");
            }
            else
            {

                

                // If the user does not have an account, then ask the user to create an account.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["ProviderDisplayName"] = info.ProviderDisplayName;
                var email = info.Principal.FindFirst(ClaimTypes.Email);
                return LocalRedirect(returnUrl);
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> UsernameExists(string username)
        {
            bool existed = false;
            var user= await _userManager.FindByNameAsync(username);
            if (user!=null)
            {
                existed = true;
            }
            return Json(existed); ;
        }

        public async Task<IActionResult> EmailExists(string email)
        {
            bool existed = false;
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                existed = true;
            }
            return Json(existed);
        }
    }
}
