using System.Net.WebSockets;
using System.Text;

namespace Client.Managers
{
    public class WebSocketManager : IDisposable
    {
        public bool isConnected { get; private set; }
        private readonly Thread _threadReceiveNotification;
        private ClientWebSocket _webSocket;
        private CancellationTokenSource _CTS;
        private bool _disposed = false;

        public WebSocketManager()
        {
            _threadReceiveNotification = new Thread(ReceiveNotificationAsync);
        }

        public async Task ConnectAsync(string token, string eventType)
        {
            if (!_disposed)
            {
                Console.WriteLine("Checking resources...");
                if (_webSocket != null)
                {
                    if (_webSocket.State == WebSocketState.Open)
                    {
                        return;
                    }
                    else
                    {
                        _webSocket.Dispose();
                    }
                }
                _webSocket = new ClientWebSocket();
                _webSocket.Options.SetRequestHeader("Authorization", $"Bearer {token}");

                if (_CTS != null)
                {
                    _CTS.Dispose();
                }
                _CTS = new CancellationTokenSource();

                try
                {
                    Console.WriteLine("Trying to connect...");
                    await _webSocket.ConnectAsync(new Uri($"wss://localhost:7192/api/wbs?EventType={eventType}"), _CTS.Token);
                }
                catch
                {
                    Console.WriteLine("Connection failed");
                    throw;
                }
                isConnected = true;
                Console.WriteLine("Connection was established");
                if (_threadReceiveNotification.ThreadState != ThreadState.Running)
                {
                    _threadReceiveNotification.Start();
                }
            }

        }

        public async Task DisconnectAsync()
        {
            if (!_disposed)
            {
                if (_webSocket is null)
                {
                    return;
                }

                if (_webSocket.State == WebSocketState.Open)
                {
                    Console.WriteLine("DDisconecting...");
                    _CTS.Cancel();
                    await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "close", CancellationToken.None);
                }
                isConnected = false;
                _webSocket.Dispose();
                _webSocket = null;
                _CTS.Dispose();
                _CTS = null;
                Console.WriteLine("DDisconected...");
            }
        }

        private async void ReceiveNotificationAsync()
        {
            try
            {
                while (_webSocket.State == WebSocketState.Open)
                {

                    ArraySegment<byte> bytesReceived = new ArraySegment<byte>(new byte[1024 * 4]);
                    WebSocketReceiveResult result = await _webSocket.ReceiveAsync(bytesReceived, CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        break;
                    }
                    var response = Encoding.UTF8.GetString(bytesReceived.Array, 0, result.Count);
                    Console.WriteLine("\n*----------------------------New Event Was Posted----------------------------*\n\n");
                    Console.WriteLine(response);
                    Console.WriteLine("\n*----------------------------------------------------------------------------*\n\n");
                }
            }
            catch (WebSocketException ex)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Unhandled exception was cathced");
                Console.WriteLine("The server suddenly closed the connection");
                Console.WriteLine(ex.Message);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine("Press 'Esc' key to continue");
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                if (_webSocket != null)
                {
                    DisconnectAsync().Wait();
                }
                _disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
