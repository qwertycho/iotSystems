using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using iotServer.classes;

namespace iotServer.Controllers
{
    public class DeviceController : Controller
    {

        private DeviceModel deviceModel = new DeviceModel();

        public IActionResult Index()
        {
            return View();
        }

        public JsonResult GetDevices()
        {
            return Json(deviceModel.getAllDevicesAsync().Result);
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
                    throw new Exception("Invalid device");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(false);
            }
        }

    }
}