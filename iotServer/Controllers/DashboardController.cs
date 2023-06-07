using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using iotServer.classes;
using System.Diagnostics;
using iotServer.NewsLetter;
using System.Text;
using System.Text.Json;
using iot.ws;

using System.Net.WebSockets;

namespace iotServer.Controllers
{
    public class DashboardController : Controller
    {

        private readonly ILogger<DashboardController> _logger;
        private DeviceModel deviceModel;
        private readonly NewsLetter.NewsLetter _newsLetter ;

        public DashboardController(ILogger<DashboardController> logger, NewsLetter.NewsLetter newsletter)
        {
            _newsLetter = newsletter;
            _logger = logger;
            deviceModel = new DeviceModel();
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> Groups()
        {
            try
            {
                List<Group> groups = await deviceModel.GetAllGroupsAsync();
                return Json(groups);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in Groups");
                return Json(new List<Group>());
            }
        }

        public JsonResult Devices()
        {
            try
            {
                return Json(deviceModel.getAllDevicesAsync().Result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in Devices");
                return Json(new List<Device>());
            }
        }

        public async Task<JsonResult> NewDevices()
        {
            try
            {
                return Json(await deviceModel.GetNewDevicesAsync());
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in NewDevices");
                return Json(new List<Device>());
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

            WS sock = new WS(webSocket);

            _newsLetter.sensorUpdate += async (object? sender, SensorUpdateEventArgs e) => {
                await sock.SendAsync(System.Text.Json.JsonSerializer.Serialize(e));
            };

            var buffer = new byte[1024 * 4];

            var receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), 
                CancellationToken.None
            );

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
    }
}
