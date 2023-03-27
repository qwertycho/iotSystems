using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using iotServer.classes;
using System.Diagnostics;

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
            Stopwatch   stopwatch = new Stopwatch();
            stopwatch.Start();

            List<Group> groups = await deviceModel.GetAllGroupsAsync();

            stopwatch.Stop();
            Console.WriteLine("Time elapsed for Groups: {0}", stopwatch.Elapsed);
            
            return Json(groups);
        }

        public JsonResult Devices()
        {           
            return Json(deviceModel.getAllDevicesAsync().Result);
        }

        public async Task<JsonResult> NewDevices()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            List<Device> devices = await deviceModel.GetNewDevicesAsync();
            stopwatch.Stop();
            Console.WriteLine("Time elapsed for NewDevices: {0}", stopwatch.Elapsed);

            return Json(devices);
        }
    }
}