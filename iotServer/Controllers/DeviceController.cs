using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using iotServer.classes;

// loggen
// using Microsoft.Extensions.Logging;

namespace iotServer.Controllers
{
    public class DeviceController : Controller
    {
        private readonly ILogger<DeviceController> _logger;

        private DeviceModel deviceModel;

        public DeviceController(ILogger<DeviceController> logger)
        {
            _logger = logger;
            deviceModel = new DeviceModel();
        }

        public JsonResult GetDevices()
        {
            return Json(deviceModel.getAllDevicesAsync().Result);
        }

        public async Task<IActionResult> details()
        {
            try
            {
                int id = Convert.ToInt32(Request.Query["id"]);
                Device device = await deviceModel.GetDeviceByID(id);


                ViewBag.deviceName = device.Name;
                ViewBag.deviceID = device.Id;
                ViewBag.deviceUUID = device.Uuid;
                ViewBag.deviceSensors = device.Sensors;
                ViewBag.deviceGroup = device.Group;
                ViewBag.deviceDate = device.Date;
                
                return View();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in details");
                return View();
            }
        }

        /// <summary>
        /// Deze knaap wordt aangeroepen als een nieuwe device zich aanmeldt
        /// De functie krijgt een uuid (mac adres) en een lijst met sensoren mee
        /// Daarvoor is de [FromBody] nodig
        /// </summary>
        public async Task<JsonResult> Init([FromBody] NewDevice device)
        {
            try
            {
                if (device.Uuid != null || device.Sensors != null)
                {
                    await deviceModel.initDevice(device);
                    return Json(true);
                }
                else
                {
                    _logger.LogError("Device Init failed, uuid or sensors is null");
                    return Json(false);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in Init");
                return Json(false);
            }
        }

        /// <summary>
        /// Haalt een list van nieuwe devices op uit de database
        /// </summary>
        public async Task<JsonResult> GetNewDevices()
        {
            try
            {
                return Json(await deviceModel.GetNewDevicesAsync());
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }

        }

    }
}