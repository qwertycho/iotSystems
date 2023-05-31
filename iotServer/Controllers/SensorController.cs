using iotServer.classes;
using iotServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace iotServer.Controllers
{
   public class SensorController : Controller
  {
    private readonly ILogger _logger;
    private readonly SensorModel _sensorModel;
    private NewsLetter.NewsLetter  _newsLetter;

    public SensorController(ILogger logger, NewsLetter.NewsLetter newsLetter)
    {
      _logger = logger;
      _sensorModel = new SensorModel();
      _newsLetter = newsLetter;
    }

    public async Task<JsonResult> temp([FromBody] SensorValue data)
    {
      return Json("jup");
    }
  }
}
