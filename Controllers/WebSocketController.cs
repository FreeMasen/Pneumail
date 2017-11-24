using System;
using System.Collections.Generic;
using System.Text;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebSockets;

namespace Pneumail.Controllers
{
    public class WebSocketController : Controller
    {
        private WebSocket WebSocket;
        public WebSocketController()
        {

        }


        public async Task Sock()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest) {
                WebSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                var msg = System.Text.Encoding.ASCII.GetBytes("Connected");
                await SendUpdate(msg);
            }
        }

        public async Task SendUpdate(byte[] update) {
            await WebSocket.SendAsync(new ArraySegment<byte>(update), WebSocketMessageType.Text, false, CancellationToken.None);
        }
    }
}