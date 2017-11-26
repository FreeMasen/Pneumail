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
                var user = await _userManager.GetUserAsync(HttpContext.User);
                await SendUpdate(user.Categories);
                while (!WebSocket.CloseStatus.HasValue) {

                }
            }
        }

        public async Task SendUpdate(List<Category> updates) {
            var mapped = JsonConvert.SerializeObject(updates);
            var msg = System.Text.Encoding.ASCII.GetBytes(mapped.ToString());
            await WebSocket.SendAsync(new ArraySegment<byte>(msg),
                                        WebSocketMessageType.Text,
                                        false, CancellationToken.None);
        }
    }
}