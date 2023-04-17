using iotServer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using iotServer.classes;

namespace iotServer.Controllers
{
    public class SetupController : Controller
    {

        private readonly ILogger<SetupController> _logger;
        private SetupModel setupModel;

        public SetupController(ILogger<SetupController> logger)
        {
            _logger = logger;
            setupModel = new SetupModel();
        }
        public async Task<IActionResult> Index()
        {
            int id = Convert.ToInt32(Request.Query["id"]);
            DeviceSetup setup = await setupModel.GetDeviceSetupAsync(id);

            ViewData["id"] = setup.id;
            ViewData["aanTijd"] = setup.aanTijd;
            ViewData["uitTijd"] = setup.uitTijd;
            ViewData["maxTemp"] = setup.maxTemp;
            ViewData["minTemp"] = setup.minTemp;

            return View();
        }

        public async Task<IActionResult> Save()
        {

            var form = HttpContext.Request.Form;

            DeviceSetup setup = new DeviceSetup();
            try
            {
                setup.id = Convert.ToInt32(form["id"]);
                setup.aanTijd = Convert.ToInt32(form["aanTijd"]);
                setup.uitTijd = Convert.ToInt32(form["uitTijd"]);
                setup.maxTemp = float.Parse(form["maxTemp"]);
                setup.minTemp = float.Parse(form["minTemp"]);

                Console.WriteLine(setup.id);
                Console.WriteLine(setup.aanTijd);
                Console.WriteLine(setup.uitTijd);
                Console.WriteLine(setup.maxTemp);
                Console.WriteLine(setup.minTemp);

                await setupModel.SaveSetupAsync(setup);

            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                // return View();
            }



            return Redirect($"Index?id={form["id"]}");
        }

        public async Task<JsonResult> GetSetup()
        {
            try
            {
                List<DeviceSetup> setups = await setupModel.GetSetupsAsync();
                return Json(setups);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in details");
                return Json(e.Message);
            }
        }
    }
}