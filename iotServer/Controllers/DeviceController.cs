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

        public JsonResult getDeviceValue(int id){

            DeviceModel.SensorValue res = deviceModel.getDeviceValue(id);

            return Json(res);
        }

        public JsonResult Init(string macadres)
        {
            return Json(true);
        }

    }
}