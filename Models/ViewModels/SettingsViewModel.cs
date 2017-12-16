using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Pneumail.Models;
namespace Pneumail.ViewModels
{
    public class SettingsViewModel
    {
        public List<EmailServiceViewModel> Services { get; set; }

        public SettingsViewModel() {
            Services = new List<EmailServiceViewModel>();
        }
        public SettingsViewModel(User user) {
            Services = user.Services.Select(s => new EmailServiceViewModel(s)).ToList();
        }
    }

    public class EmailServiceViewModel
    {
        public Guid? Id { get; set; }
        [RegularExpression("(http(s)?://)?([a-zA-Z0-9]+)?.[a-zA-Z0-9]+.[a-zA-Z0-9]+", ErrorMessage="Please enter a valid URL")]
        public string Address { get; set; }
        public int Port { get; set; }
        [EmailAddress]
        public string Username { get; set; }
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public bool Hidden { get; set; }

        public EmailServiceViewModel()
        {
            Id = null;
            Address = "";
            Port = 0;
            Username = "";
            Password = "";
            Hidden = true;
        }

        public EmailServiceViewModel(EmailService service)
        {
            Id = service.Id;
            Address = service.InboundAddress;
            Port = service.InboundPort;
            Username = service.Username;
            Password = service.Password;
        }

    }
}