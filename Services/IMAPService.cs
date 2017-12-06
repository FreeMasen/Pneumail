using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using MailKit.Net.Imap;
using MailKit.Security;
using MailKit;
using Pneumail.Models;

namespace Pneumail.Services 
{
    public class IMAPService
    {
        private EmailService Service;
        private ImapClient Client;
        public IMAPService()
        {
            // this.Service = service;
            this.Client = new ImapClient();
            Client.Connected += Connected;
            try {
                this.Client.Connect("imap.gmail.com", 993, SecureSocketOptions.SslOnConnect);
            } catch (Exception e) {
                Log($"Error building client {e}");
            }
        }

        public async void Connected(Object sender, EventArgs e) {
            try {
                Log($"Connected:\n{sender}\n{e}");
                Client.Authenticated += Authenticated;
                //create credential here
                await this.Client.AuthenticateAsync(
                    System.Text.Encoding.UTF8,
                    c);
            } catch (Exception err) {
                Log($"Error Authenticating {err.Message}");
            }
        }

        public async void Authenticated(Object sender, EventArgs e) {
            try {
                Log($"Authenticated:\n{sender}\n{e}");
                Client.Inbox.Subscribed += Subscribed;

                var folders = await Client.GetFoldersAsync(Client.PersonalNamespaces.First());
                foreach (var folder in folders) {
                    Log($"Folder: {folder.FullName}, {folder.Count()}");
                    folder.Open(FolderAccess.ReadOnly);
                    foreach (var msg in folder) {
                        Log($"Message: {msg.MessageId}: {msg.Subject}");
                    }
                }
            } catch (Exception err) {
                Log($"Error subscribing: {err}");
            }
        }

        private void Alert(Object sender, EventArgs e) {
            Log($"Alert:\n{sender}\n{e}");
        }

        private void Subscribed(Object sender, EventArgs e) {
            try {
                Client.Alert += Alert;
            } catch (Exception ex) {
                Log($"Error {ex}");
            }
        }

        private void Log(string msg) {
            Console.BackgroundColor = System.ConsoleColor.DarkRed;
            Console.ForegroundColor = System.ConsoleColor.White;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
    }
}