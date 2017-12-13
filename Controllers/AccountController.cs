using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Pneumail.Data;
using Pneumail.Models;
using Pneumail.ViewModels;
using Pneumail.Services;

namespace Pneumail.Controllers
{
    /// <summary>
    /// The Controller that deals with Account requests
    /// </summary>
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _data;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ApplicationDbContext data)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _data = data;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            return View();
        }
        /// <summary>
        /// Action associated with submitting the login form
        /// </summary>
        /// <param name="model">
        /// The view model containing the username and password
        /// entered by the user
        /// </param>
        /// <returns>Http response to a POST Request</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }
            return View(model);
        }
        /// <summary>
        /// Http route for getting the /Account/Register path
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// The Action associated with submitting the registration form
        /// </summary>
        /// <param name="model">
        /// The view model containing the user's account information
        /// </param>
        /// <returns>The Http response to the POST request</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {

                var result = await CreateUser(model);
                if (result.Succeeded) {
                    return RedirectToAction("Login", "Account");
                } else {
                    foreach (var e in result.Errors) {
                        ModelState.AddModelError(e.Code, e.Description);
                    }
                    return View(model);
                }
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Logout() {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _data.Users.Where(u => u.Id == userId)
                                    .Include(u => u.Services)
                                    .FirstAsync();
            if (user != null) {
                return View(new SettingsViewModel(user));
            }
            return View(new SettingsViewModel());
        }

#region helpers
        public async Task<IdentityResult> CreateUser(RegisterViewModel model)
        {
            var user = new User() {
                UserName = model.Username,
                Categories = new List<Category>() {
                    new Category() {
                        Messages = new List<Message>(),
                        Name = "Sent",
                    },
                    new Category() {
                        Messages = new List<Message>(),
                        Name = "Incomplete"
                    },
                    new Category() {
                        Messages = new List<Message>(),
                        Name = "Complete"
                    }
                }
            };
            return await _userManager.CreateAsync(user, model.Password);
        }

#endregion
    }
}
