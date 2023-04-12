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
        public IActionResult Index()
        {
            _logger.LogInformation("Index page says hello");
            return View();
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