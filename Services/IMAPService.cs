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
        private static SortedSet<Guid> CurrentlyFetching = new SortedSet<Guid>();
        public IMAPService()
        {

        }

        public async Task<List<Message>> GetMessages(EmailService service)
        {
            var ret = new List<Message>();
            try {
                if (!IMAPService.CurrentlyFetching.Add(service.Id)) {
                    Console.WriteLine("Already fetching that service");
                    return ret;
                }
                var client = new ImapClient();
                await client.ConnectAsync(service.Address, service.Port, SecureSocketOptions.SslOnConnect);
                if (client.IsConnected)
                {
                    Console.WriteLine("Connected!");
                    await client.AuthenticateAsync(Encoding.UTF8, service.Credentials());
                    if (client.IsAuthenticated)
                    {
                        Console.WriteLine("Authenticated!");
                        foreach (var folder in await client.GetFoldersAsync(client.PersonalNamespaces.First()))
                        {
                            Console.WriteLine($"trying to open {folder.Name}");
                            await folder.OpenAsync(FolderAccess.ReadOnly);
                            Console.WriteLine($"adding {folder.Count()} messages");
                            ret = ret.Concat(folder.Select(m => MapMessage(m))).ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            } 
            finally 
            {
                IMAPService.CurrentlyFetching.Remove(service.Id);
            }
            return ret;
        }

        private bool AddFetching(Guid newId) {
            if (IMAPService.CurrentlyFetching.Contains(newId)) {
                return false;
            }
            return IMAPService.CurrentlyFetching.Add(newId);
            
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
            try {
                var ret = new Message();
                ret.Date = from.Date.Date;
                ret.Sender = from.From.Select(s => new EmailAddress(s.ToString(), s.Name)).First();
                ret.Subject = from.Subject;
                ret.Recipients = from.To.Select(r => new EmailAddress(r.ToString(),r.Name)).ToList();
                ret.Copied = from.Cc.Select(r => new EmailAddress(r.ToString(), r.Name)).ToList();
                ret.BlindCopied = from.Bcc.Select(r => new EmailAddress(r.ToString(), r.Name)).ToList();
                ret.Content = from.HtmlBody != null ? from.HtmlBody : from.TextBody;
                    //todo: Add attachments
                return ret;
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }
    }
}