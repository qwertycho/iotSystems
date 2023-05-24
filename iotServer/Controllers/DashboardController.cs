using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using iotServer.classes;
using System.Diagnostics;

namespace iotServer.Controllers
{
    public class DashboardController : Controller
    {

        private readonly ILogger<DashboardController> _logger;
        private DeviceModel deviceModel;

        public DashboardController(ILogger<DashboardController> logger)
        {
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
    }
}
