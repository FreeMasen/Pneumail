using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Pneumail.Data;
using Pneumail.Models;

namespace Pneumail.Controllers
{
    /// <summary>
    /// The controller that will be used to maintin and interact with the WebSocket
    /// connection to the browser
    /// </summary>
    public class WebSocketController : Controller
    {
        private WebSocket WebSocket;
        private ApplicationDbContext _data;
        private UserManager<User> _userManager;
        public WebSocketController(ApplicationDbContext data,
                                    UserManager<User> userManager)
        {
            this._data = data;
            this._userManager = userManager;
        }

        /// <summary>
        /// The socket route
        /// </summary>
        public async Task Sock()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest) {
                WebSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                Console.WriteLine("Accepted Websocket");
                var userId = _userManager.GetUserId(User);
                Console.WriteLine($"Got UserId: {userId}");
                var user = await _data.Users.Where(u => u.Id == userId)
                                            .Include(u => u.Categories)
                                                .ThenInclude(c => c.Messages)
                                                    .ThenInclude(m => m.Recipients)
                                            .Include(u => u.Categories)
                                                .ThenInclude(c => c.Messages)
                                                    .ThenInclude(m => m.BlindCopied)
                                            .Include(u => u.Categories)
                                                .ThenInclude(c => c.Messages)
                                                    .ThenInclude(m => m.Copied)
                                            .Include(u => u.Categories)
                                                .ThenInclude(c => c.Messages)
                                                    .ThenInclude(m => m.Sender)
                                            .Include(u => u.Categories)
                                                .ThenInclude(c => c.Messages)
                                                    .ThenInclude(m => m.Attachments)
                                            .FirstOrDefaultAsync();
                Console.WriteLine($"Got User Data");
                try {
                    var rawBuf = new byte[1024 * 4];
                    var buffer = new ArraySegment<byte>(rawBuf);
                    var update = new UpdateMessage() {
                                        Initial = new List<Update>() {
                                            new Update() {
                                                Categories = user.Categories,
                                                KeysModified = new List<string>(),
                                            },
                                        }
                                    };
                    Console.WriteLine($"Build update");
                    await SendUpdate(update);
                    Console.WriteLine($"Sent Update");
                    var result = await WebSocket.ReceiveAsync(buffer, CancellationToken.None);
                    Console.WriteLine($"Recieved initial message");
                    while (!result.CloseStatus.HasValue)
                    {
                        if (result.Count > 0)
                        {
                            var msgText = System.Text.Encoding.ASCII.GetString(buffer.Array.Take(result.Count).ToArray());
                            Console.WriteLine($"Websocket Message\n----------\n{msgText}");
                        }
                        result = await WebSocket.ReceiveAsync(buffer, CancellationToken.None);
                    }
                    await WebSocket.CloseAsync(result.CloseStatus.Value,
                                                result.CloseStatusDescription, CancellationToken.None);
                } catch (Exception e)
                {
                    Console.WriteLine($"error in websocket: {e.Message}");
                }
            }
        }

        public async Task SendUpdate(UpdateMessage updates) {
            try {
                var mapped = JsonConvert.SerializeObject(
                                                updates,
                                                Formatting.None,
                                                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                Console.WriteLine("Parsed update to json");
                var msg = System.Text.Encoding.ASCII.GetBytes(mapped);
                Console.WriteLine("Converted to bytes");
                var bytes = new ArraySegment<byte>(msg, 0, msg.Count());
                Console.WriteLine("Converted Array Buffer");
                await WebSocket.SendAsync(bytes,
                                            WebSocketMessageType.Text,
                                            true, CancellationToken.None);
                Console.WriteLine("Sent");
            }
            catch (Exception e) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {e.Message}");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}