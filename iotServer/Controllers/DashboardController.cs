using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using iotServer.classes;

namespace iotServer.Controllers
{
    public class DashboardController : Controller
    {
        private DeviceModel deviceModel = new DeviceModel();
        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> Groups()
        {
            List<DeviceModel.Group> groups = await deviceModel.GetAllGroupsAsync();
            return Json(groups);
        }

        public JsonResult Devices()
        {           
            return Json(deviceModel.getAllDevicesAsync().Result);
        }
    }
}