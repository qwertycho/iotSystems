using iotServer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;


namespace iotServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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

        private static async Task Echo(WebSocket webSocket)
        {


            int parxe(string message)
            {
                Console.WriteLine(message);
                if (message.StartsWith("ding: "))
                {
                    return int.Parse(message.Substring(6));
                }
                throw new Exception("Invalid message");
            }


            var buffer = new byte[1024 * 4];

            var receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);

            var sendBuffer = Encoding.UTF8.GetBytes("Hello from the server");

            while (!receiveResult.CloseStatus.HasValue)
            {
                Console.WriteLine("Received: " + Encoding.UTF8.GetString(buffer));


                try
                {
                int number = parxe(Encoding.UTF8.GetString(buffer));
                sendBuffer = Encoding.UTF8.GetBytes(number.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                sendBuffer = Encoding.UTF8.GetBytes("Invalid message");
                }

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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}