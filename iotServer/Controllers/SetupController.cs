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
            ViewData["deviceID"] = setup.deviceID;
            ViewData["aanTijd"] = setup.aanTijd;
            ViewData["uitTijd"] = setup.uitTijd;
            ViewData["maxTemp"] = setup.maxTemp;
            ViewData["minTemp"] = setup.minTemp;

            return View();
        }

        public async Task<IActionResult> Save()
        {
            try
            {
                DeviceSetup setup = setupModel.generateSetupFromForm(Request.Form);

                await setupModel.SaveSetupAsync(setup);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                // return View();
            }

            return Redirect($"Index?id={Request.Form["deviceID"]}");
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
