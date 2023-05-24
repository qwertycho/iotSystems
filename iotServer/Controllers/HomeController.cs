using iotServer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using iotServer.NewsLetter;


namespace iotServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly NewsLetter.NewsLetter _newsLetter;

        public HomeController(ILogger<HomeController> logger, NewsLetter.NewsLetter newsLetter)
        {
            _logger = logger;
            _newsLetter = newsLetter;
        }

        public IActionResult Index()
        {

            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                return View();
            }
            else
            {
                return View();
            }
        }

        public async Task ws()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await Echo(webSocket);
            }
            else
            {
                HttpContext.Response.StatusCode = 400;
            }
        }

        private async Task Echo(WebSocket webSocket)
        {

            _newsLetter.sensorUpdate += async (object? sender, SensorUpdateEventArgs e) => {

                await webSocket.SendAsync(
                new ArraySegment<byte>(Encoding.UTF8.GetBytes(e.value), 0, Encoding.UTF8.GetBytes(e.value).Length),
                messageType: WebSocketMessageType.Text,
                endOfMessage: true,
                CancellationToken.None
                );
            };

            var buffer = new byte[1024 * 4];

            var receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);

            var sendBuffer = Encoding.UTF8.GetBytes("Hello from the server");

            while (!receiveResult.CloseStatus.HasValue)
            {
                Console.WriteLine("Received: " + Encoding.UTF8.GetString(buffer));

               buffer = new byte[1024 * 4];

                await webSocket.SendAsync(
                new ArraySegment<byte>(sendBuffer, 0, sendBuffer.Length),
                messageType: WebSocketMessageType.Text,
                endOfMessage: true,
                CancellationToken.None
                );

                receiveResult = await webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), CancellationToken.None);

            }

            await webSocket.CloseAsync(
            WebSocketCloseStatus.NormalClosure,
            "lol",
            CancellationToken.None);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task ws2()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                Console.WriteLine("ws2");
                await Echo2(webSocket);
            }
            else
            {
                HttpContext.Response.StatusCode = 400;
            }
        }

        private async Task Echo2(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];

            var receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);

            var sendBuffer = Encoding.UTF8.GetBytes("Hello from the server");

            while (!receiveResult.CloseStatus.HasValue)
            {
                Console.WriteLine("Received2: " + Encoding.UTF8.GetString(buffer));


                   int randVal = new Random().Next(0, 100);

            _newsLetter.OnSensorUpdate(new SensorUpdateEventArgs { deviceID = 1, value= "2", sensor= "3" });

                buffer = new byte[1024 * 4];

                await webSocket.SendAsync(
                new ArraySegment<byte>(sendBuffer, 0, sendBuffer.Length),
                messageType: WebSocketMessageType.Text,
                endOfMessage: true,
                CancellationToken.None
                );

                receiveResult = await webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), CancellationToken.None);

            }

            await webSocket.CloseAsync(
            WebSocketCloseStatus.NormalClosure,
            "lol",
            CancellationToken.None);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
