using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace Server.API.Controllers
{
    //[ApiController]
    public class WBSController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("/ws")]
        public async Task<IActionResult> Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync(
                    new WebSocketAcceptContext { DangerousEnableCompression = true });
                var buffer = new byte[1024 * 4];
                var receiveResult = await webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), CancellationToken.None);

                while (!receiveResult.CloseStatus.HasValue)
                {
                    await webSocket.SendAsync(
                        new ArraySegment<byte>(buffer, 0, receiveResult.Count),
                        receiveResult.MessageType,
                        receiveResult.EndOfMessage,
                        CancellationToken.None);

                    receiveResult = await webSocket.ReceiveAsync(
                        new ArraySegment<byte>(buffer), CancellationToken.None);
                }

                await webSocket.CloseAsync(
                    receiveResult.CloseStatus.Value,
                    receiveResult.CloseStatusDescription,
                    CancellationToken.None);

                return Ok();
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                return BadRequest();
            }
        }

        
    }
}
