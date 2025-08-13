using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ShoppingList.DataAccess;
using ShoppingList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShoppingList.Controllers {
    public class AuthController : Controller {

        private readonly SettingsPersistence _settingsPersistence;

        public AuthController(SettingsPersistence settingsPersistence) {
            _settingsPersistence = settingsPersistence;
        }

        public IActionResult Login() {
            return View();
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string returnUrl, string password) {
            var settings = _settingsPersistence.Load();
            if (password == settings.Password) {
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, "ShoppingList", ClaimValueTypes.String, "https://shoppinglist.com")
                };
                
                var userIdentity = new ClaimsIdentity(claims, "SecureLogin");
                var userPrincipal = new ClaimsPrincipal(userIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    userPrincipal,
                    new AuthenticationProperties {
                        IsPersistent = true,
                        AllowRefresh = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddDays(365) // make sure it's DateTimeOffset
                    });
                return GoToReturnUrl(returnUrl);
            }

            return View("denied");
        }

        private IActionResult GoToReturnUrl(string returnUrl) {
            if (Url.IsLocalUrl(returnUrl)) {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Start", "ShoppingList");
        }
    }
}
