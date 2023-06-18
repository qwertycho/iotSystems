using Microsoft.AspNetCore.Mvc;
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
                int id = int.Parse(Request.Query["id"]);
                Device device = await deviceModel.GetDeviceByID(id);
                List<Group> groepen = await deviceModel.GetAllGroupsAsync();
                ViewBag.groups = groepen; 
                ViewBag.deviceName = device.Name;
                ViewBag.deviceID = device.Id;
                ViewBag.deviceUUID = device.Uuid;
                ViewBag.deviceSensors = device.Sensors;
                ViewBag.deviceGroup = device.Group;
                ViewBag.deviceDate = device.Date;
                ViewBag.deviceStatus = device.Status;
                return View();
            }
            catch (Exception e)
            {
              _logger.LogError("fout in details: " + e.Message);
                return View();
            }
        }

        /// <summary>
        /// Deze knaap wordt aangeroepen als een nieuwe device zich aanmeldt
        /// De functie krijgt een uuid (mac adres) en een lijst met sensoren mee
        /// Daarvoor is de [FromBody] nodig
        /// </summary>
        /// <param name="device"></param>
        /// <returns>Boolean voor het succesvol ophalen of toegoegen device</returns>
        public async Task<JsonResult> Init([FromBody] NewDevice device)
        {
            try
            {
                deviceModel.validateNewDevice(device);
                DeviceSetup deviceSetup = await deviceModel.initDevice(device);

                if(deviceSetup.id != 0)
                {
                  await deviceModel.setStatus(device.Uuid, "A");
                }
                  await deviceModel.setStatus(device.Uuid, "N");

                return Json(deviceSetup);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, "Error in init");
                DeviceSetup deviceSetup = new DeviceSetup();
                deviceSetup.status = false;
                deviceSetup.error = true;
                return Json(deviceSetup);
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

        public async Task<IActionResult> UpdateDevice(string deviceID, string groupID, string deviceName, string deviceStatus)
        {
          try{
            int id = int.Parse(deviceID);
            int group = int.Parse(groupID);
            if(deviceName != "" && deviceName != null)
            {
              await deviceModel.UpdateDeviceName(id, deviceName);
            }
            if(group != 0 && id != 0)
            {
              await deviceModel.UpdateDeviceGroup(id, group);
            }

            return Redirect("/device/details?id=" + deviceID);
          } catch(Exception e)
          {
            _logger.LogError(e.Message);
            return Redirect("/device/details?id=" + deviceID);
          }
        }

        public async Task<IActionResult> delete(string deviceID)
        {

          try{
            int id = int.Parse(deviceID);
            await deviceModel.deleteDevice(id);
            return Redirect("/dashboard");
          } catch(Exception e)
          {
            _logger.LogError(e.Message);
            return Redirect("/dashboard");
          }
        }
    }
}
