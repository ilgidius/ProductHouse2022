using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Common.Classes.Models.EventModels;
using Server.Common.Interfaces.Models.IEventModel;
using System.ComponentModel.DataAnnotations;
using System.Net.WebSockets;
using System.Text;

namespace Server.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class WBSController : ControllerBase
    {
        private readonly IEventNotifyService _eventNotifyService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventNotifyService"></param>
        public WBSController(IEventNotifyService eventNotifyService)
        {
            _eventNotifyService = eventNotifyService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("/ws")]
        public async Task Get([FromHeader] EventSubscriptionHeaders headers)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync(
                    new WebSocketAcceptContext { DangerousEnableCompression = true });
                var buffer = new byte[1024 * 10];
                var receiveResult = await webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), CancellationToken.None);

                _eventNotifyService.AddSubscriber(headers.Login, headers.EventType);

                while (!receiveResult.CloseStatus.HasValue)
                {
                    string message = await _eventNotifyService.PublishAsync(headers.Login);
                    await webSocket.SendAsync(
                        new ArraySegment<byte>(Encoding.UTF8.GetBytes(message)),
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None);

                    receiveResult = await webSocket.ReceiveAsync(
                        new ArraySegment<byte>(buffer), CancellationToken.None);
                }

                _eventNotifyService.RemoveSubscriber(headers.Login);

                await webSocket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    receiveResult.CloseStatusDescription,
                    CancellationToken.None);

                //HttpContext.Response.StatusCode = StatusCodes.Status200OK;
                //return Ok();
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                //return BadRequest();
            }
        }

        
    }
}
