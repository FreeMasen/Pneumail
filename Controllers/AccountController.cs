using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pneumail.Data;
using Pneumail.Models;

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
                var user = new User() {
                    UserName = model.Username,
                };
                var result = await _userManager.CreateAsync(user, model.Password);
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

        public async Task<IActionResult> Seed()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var incomplete = new Category() {
                Name = "Incomplete",
                Messages = new List<Message>(),
            };
            user.Categories = new List<Category>();
            for (var i = 0; i < 10; i++)
            {
                incomplete.Messages.Add(new Message(){
                    Sender = new EmailAddress() {
                        Username = "sender",
                        Host = "mail",
                        Domain = "com"
                    },
                    Recipients = new List<EmailAddress>() {
                        new EmailAddress() {
                            Username = "recipient",
                            Host = "mail",
                            Domain = "com"
                        }
                    },
                    Subject = $"{i}: The quick brown fox jumpped over the lazy dog",
                    Content = @"Sed ut perspiciatis, unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam eaque ipsa, quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt, explicabo. Nemo enim ipsam voluptatem, quia voluptas sit, aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos, qui ratione voluptatem sequi nesciunt, neque porro quisquam est, qui dolorem ipsum, quia dolor sit amet consectetur adipisci[ng] velit, sed quia non numquam [do] eius modi tempora inci[di]dunt, ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur? Quis autem vel eum iure reprehenderit, qui in ea voluptate velit esse, quam nihil molestiae consequatur, vel illum, qui dolorem eum fugiat, quo voluptas nulla pariatur?

                                At vero eos et accusamus et iusto odio dignissimos ducimus, qui blanditiis praesentium voluptatum deleniti atque corrupti, quos dolores et quas molestias excepturi sint, obcaecati cupiditate non provident, similique sunt in culpa, qui officia deserunt mollitia animi, id est laborum et dolorum fuga. Et harum quidem rerum facilis est et expedita distinctio. Nam libero tempore, cum soluta nobis est eligendi optio, cumque nihil impedit, quo minus id, quod maxime placeat, facere possimus, omnis voluptas assumenda est, omnis dolor repellendus. Temporibus autem quibusdam et aut officiis debitis aut rerum necessitatibus saepe eveniet, ut et voluptates repudiandae sint et molestiae non recusandae. Itaque earum rerum hic tenetur a sapiente delectus, ut aut reiciendis voluptatibus maiores alias consequatur aut perferendis doloribus asperiores repellat…",
                    Attachments = new List<Attachment>(),
                    BlindCopied = new List<EmailAddress>(),
                    Copied = new List<EmailAddress>()
                });
            }
            user.Categories.Add(incomplete);
            user.Services = new List<EmailService>();

            await _userManager.UpdateAsync(user);
            // _data.Update(user);
            // await _data.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
