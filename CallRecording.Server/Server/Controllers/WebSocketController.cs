using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Common.Classes.Models.Common;
using Server.Common.Classes.Models.Notification;
using Server.Common.Interfaces.Models.INotification;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;

namespace Server.API.Controllers
{
    /// <summary>
    /// Controller for WebSockets
    /// </summary>
    [Route("api/wbs")]
    [ApiController]
    public class WebSocketController : ControllerBase
    {
        private WebSocket? _webSocket;
        private SubscriptionContext? _subscriptionContext;
        private readonly ILogger<WebSocketController> _log;
        private readonly INotificationService _notificationService;
        private readonly CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// WebSockets controller constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="notificationService"></param>
        public WebSocketController(ILogger<WebSocketController> logger, INotificationService notificationService)
        {
            _log = logger;
            _notificationService = notificationService;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// Establish WebSocket connection
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="authorization"></param>
        /// <response code="200">Connection succesfully closed</response>
        /// <response code="401">The request was not sent by an authorized user</response>
        /// <response code="500">Internal server error</response>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task Get([FromQuery, RegularExpression(@"(INIT|RINGING|START|VOICE|STOP)",
            ErrorMessage = "Event type does not match possible options.")] string eventType,
            [FromHeader] string authorization)
        {
            try
            {
                if (HttpContext.WebSockets.IsWebSocketRequest)
                {
                    using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync(
                        new WebSocketAcceptContext { DangerousEnableCompression = true });

                    _webSocket = webSocket;

                    await HandleWebSocket(webSocket, authorization.Replace("Bearer ", string.Empty), eventType);
                }
                else
                {
                    HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                }
            }
            catch(Exception ex)
            {
                _log.LogError(ex.Message, ex);
                HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                _log.LogInformation($"Trying to unsubscribe '{_subscriptionContext.Login}' from {eventType} events");
                _notificationService.Unsubscribe(_subscriptionContext);
            }
            _cancellationTokenSource.Cancel();
        }

        private async Task HandleWebSocket(WebSocket webSocket, string jwtToken, string eventType)
        {
            var jwtReader = new JwtSecurityTokenHandler().ReadJwtToken(jwtToken);
            string login = jwtReader.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            
            _subscriptionContext = new SubscriptionContext()
            {
                Login = login,
                EventType = eventType,
            };
            _subscriptionContext.RiseNotificationEventAsync += EventHandlerAsync;

            _log.LogInformation($"Trying to subscribe '{login}' for {eventType} events");
            _notificationService.Subscribe(_subscriptionContext);

            if (!_notificationService.IsSubscribed(_subscriptionContext))
            {
                throw new Exception($"Subscription for {login} failed.");
            }

            var buffer = new byte[1024 * 4];
            
            try
            {
                var receiveResult = await webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), _cancellationTokenSource.Token);

                while (!receiveResult.CloseStatus.HasValue)
                {
                    Array.Clear(buffer, 0, buffer.Length);

                    receiveResult = await webSocket.ReceiveAsync(
                        new ArraySegment<byte>(buffer), _cancellationTokenSource.Token);

                    if (receiveResult.MessageType == WebSocketMessageType.Close)
                    {
                        break;
                    }
                }

                _log.LogInformation($"Trying to unsubscribe '{_subscriptionContext.Login}' from {eventType} events");
                _notificationService.Unsubscribe(_subscriptionContext);

                _log.LogInformation($"Close connection for '{login}'");
                await webSocket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    receiveResult.CloseStatusDescription,
                    _cancellationTokenSource.Token);
            }
            catch (WebSocketException ex)
            {
                _log.LogError("The client suddenly closed the connection");
                Console.WriteLine(ex.Message);
                _log.LogInformation($"Trying to unsubscribe '{_subscriptionContext.Login}' from {eventType} events");
                _notificationService.Unsubscribe(_subscriptionContext);
            }
            catch
            {
                throw;
            }
        }

        private async Task EventHandlerAsync(EventModel sendEvent)
        {
            if (_webSocket.State == WebSocketState.Open)
            {
                string message = "New event was added:\nType: " + sendEvent.EventType + "\nAdded time: " + sendEvent.AddedTime +
                    "\nSent time: " + sendEvent.SentTime + "\nBussiness logic {\n\tKey: " + sendEvent.Key + "\n\tValue: " +
                    sendEvent.Value + "\n}";

                await _webSocket.SendAsync(
                    new ArraySegment<byte>(Encoding.UTF8.GetBytes(message)),
                    WebSocketMessageType.Text,
                    true,
                    _cancellationTokenSource.Token);
            }
        }
    }
}
