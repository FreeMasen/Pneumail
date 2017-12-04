using System;
using System.Threading.Tasks;
using MailKit.Net.Imap;
using MailKit.Security;
using Pneumail.Models;

namespace Pneumail.Services 
{
    public class EmailClient
    {
        private EmailService Service;
        private ImapClient Client;
        public EmailClient(EmailService service)
        {
            this.Service = service;
            this.Client = new ImapClient();
            Client.Connected += Connected;
            try {
            this.Client.Connect("imap.gmail.com", 993, SecureSocketOptions.StartTls | SecureSocketOptions.SslOnConnect);
            
            } catch (Exception e) {
                Log($"Error building client {e}");
            }
        }

        public async void Connected(Object sender, EventArgs e) {
            Log($"Connected:\n{sender}\n{e}");
            Client.Alert += Alert;
        }

        private void Alert(Object sender, EventArgs e) {
            Log($"Alert:\n{sender}\n{e}");
        }

        private void Log(string msg) {
            Console.BackgroundColor = System.ConsoleColor.DarkRed;
            Console.ForegroundColor = System.ConsoleColor.White;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
    }
}