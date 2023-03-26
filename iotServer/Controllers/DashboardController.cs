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
            deviceModel.dbTest2();

            return View();
        }

        public JsonResult Groups()
        {

            List<Group>? groups = new List<Group>();

            string file = System.IO.File.ReadAllText("data/groups.json");

            groups = JsonConvert.DeserializeObject<List<Group>>(file);

            for(int i = 0; i < groups.Count; i++){
                groups[i].devices = deviceModel.getAllDevices().Where(x => x.Group == groups[i].groupName).ToList();
            }

            return Json(groups);
        }

        public JsonResult Devices()
        {           
            return Json(deviceModel.getAllDevices());
        }

        public JsonResult getDeviceValue(int id){

            DeviceModel.SensorValue res = deviceModel.getDeviceValue(id);

            return Json(res);
        }
    }
}