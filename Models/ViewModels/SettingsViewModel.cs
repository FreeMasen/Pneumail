using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Pneumail.Models;
namespace Pneumail.ViewModels
{
    public class SettingsViewModel
    {
        public List<EmailServiceViewModel> Services { get; set; }
    }

    public class EmailServiceViewModel
    {
        public Guid? Id { get; set; }
        [Url]
        public string Address { get; set; }
        public int Port { get; set; }
        [EmailAddress]
        public string Username { get; set; }
        public string Password { get; set; }

        public EmailServiceViewModel()
        {
            Id = null;
            Address = "";
            Port = 0;
            Username = "";
            Password = "";
        }

        public EmailServiceViewModel(EmailService service)
        {
            Id = service.Id;
            Address = service.Address;
            Port = service.Port;
            Username = service.Username;
            Password = service.Password;
        }

    }
}