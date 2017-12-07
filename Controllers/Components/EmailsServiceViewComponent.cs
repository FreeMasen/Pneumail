using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pneumail.Data;
namespace Pneumail.ViewModels
{
    [ViewComponent(Name = "EmailService")]
    public class EmailsServiceViewComponent: ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public EmailsServiceViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult InvokeAsync(Guid? id = null) {
            if (id == null) {
                return View(new EmailServiceViewModel());
            }
            var service = _context.EmailServices.Where(s => s.Id == id).First();
            return View(new EmailServiceViewModel(service));
        }
    }
}