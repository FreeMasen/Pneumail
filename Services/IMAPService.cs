using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Imap;
using MailKit.Security;
using MailKit;
using Pneumail.Models;

namespace Pneumail.Services 
{
    public class IMAPService
    {
        public IMAPService()
        {

        }

        public async Task<List<Message>> GetMessages(EmailService service)
        {
            var ret = new List<Message>();
            try {
                var client = new ImapClient();
                await client.ConnectAsync(service.Address, service.Port, SecureSocketOptions.SslOnConnect);
                if (client.IsConnected)
                {
                    await client.AuthenticateAsync(Encoding.UTF8, service.Credentials());
                    if (client.IsAuthenticated)
                    {
                        foreach (var folder in await client.GetFoldersAsync(client.PersonalNamespaces.First()))
                        {
                            await folder.OpenAsync(FolderAccess.ReadOnly);
                            ret.Concat(folder.Select(m => MapMessage(m)));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
            return ret;
        }

        // public async void Connected(Object sender, EventArgs e) {
        //     try {
        //         Log($"Connected:\n{sender}\n{e}");
        //         Client.Authenticated += Authenticated;
        //         var c = new System.Net.NetworkCredential("r.f.masen@gmail.com", "Rfm3683$");
        //         await this.Client.AuthenticateAsync(
        //             System.Text.Encoding.UTF8,
        //             c);
        //     } catch (Exception err) {
        //         Log($"Error Authenticating {err.Message}");
        //     }
        // }

        // public async void Authenticated(Object sender, EventArgs e) {
        //     try {
        //         Log($"Authenticated:\n{sender}\n{e}");
        //         // Client.Inbox.Subscribed += Subscribed;
        //         var folders = await Client.GetFoldersAsync(Client.PersonalNamespaces.First());
        //         foreach (var folder in folders) {
        //             folder.Open(FolderAccess.ReadOnly);
        //             Log($"Folder: {folder.FullName}, {folder.Count()}");
        //             foreach (var msg in folder) {
        //                 Log($"Message: {msg.MessageId}: {msg.Subject}");
        //             }
        //         }
        //     } catch (Exception err) {
        //         Log($"Error subscribing: {err}");
        //     }
        // }

        // private void Alert(Object sender, EventArgs e) {
        //     Log($"Alert:\n{sender}\n{e}");
        // }

        // private void Subscribed(Object sender, EventArgs e) {
        //     try {
        //         Client.Alert += Alert;
        //     } catch (Exception ex) {
        //         Log($"Error {ex}");
        //     }
        // }

        private void Log(string msg) {
            Console.BackgroundColor = System.ConsoleColor.DarkRed;
            Console.ForegroundColor = System.ConsoleColor.White;
            Console.WriteLine(msg);
            Console.ResetColor();
        }

        private Message MapMessage(MimeKit.MimeMessage from) {
            return new Message {
                Date = from.Date.Date,
                Sender = new EmailAddress(from.Sender.Address, from.Sender.Name),
                Subject = from.Subject,
                Recipients = from.To.Select(r => new EmailAddress(r.ToString(),r.Name)).ToList(),
                Copied = from.Cc.Select(r => new EmailAddress(r.ToString(), r.Name)).ToList(),
                BlindCopied = from.Bcc.Select(r => new EmailAddress(r.ToString(), r.Name)).ToList(),
                Content = from.HtmlBody != null ? from.HtmlBody : from.TextBody,
                //todo: Add attachments
            };
        }
    }
}