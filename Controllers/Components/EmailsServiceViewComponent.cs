using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Pneumail.Data;
using Pneumail.Models;

namespace Pneumail.ViewModels
{
    [ViewComponent(Name = "EmailService")]
    public class EmailsServiceViewComponent: ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        public EmailsServiceViewComponent(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid? id = null) {
            if (id == null) {
                return View(new EmailServiceViewModel());
            }
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var service = user.Services.Where(s => s.Id == id).First();
            if (service == null) {
                //todo: Update this to have an error message...
                return View(new EmailServiceViewModel());
            }
            return View(new EmailServiceViewModel(service));
        }
    }
}