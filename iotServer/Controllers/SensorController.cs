using iotServer.classes;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using iotServer.NewsLetter;

namespace iotServer.Controllers
{
   public class SensorController : Controller
  {
    private readonly ILogger<SensorController> _logger;
    private readonly NewsLetter.NewsLetter _newsLetter;
    private DeviceModel deviceModel = new DeviceModel();    
    //    private SensorModel sensorModel = new SensorModel();

    public SensorController(ILogger<SensorController> logger, NewsLetter.NewsLetter newsLetter)
    {
      _logger = logger;
      _newsLetter = newsLetter;
    }
      

    public async Task<JsonResult> updateTemp([FromBody] SensorValue sensorValue)
    {
      

      return Json("lsp ding");
    }

  }
}
