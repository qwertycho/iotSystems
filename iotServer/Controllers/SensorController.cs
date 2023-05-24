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
    private DeviceModel deviceModel;    
//    private SensorModel sensorModel;

    public SensorController(ILogger<SensorController> logger, NewsLetter.NewsLetter newsLetter)
    {
      _logger = logger;
      _newsLetter = newsLetter;
    }

    public async Task<JsonResult> insert([FromBody] List<SensorValue> sensorValues)
    {
      Device device = await deviceModel.GetDeviceByID(sensorValues[0].id);

      return Json("ding");

      for(int i = 0; i < sensorValues.Count(); i++)
      {
        try
        {
//          await sensorModel.addValue(sensorValues[i]);
  //        _newsLetter.sensorUpdate(new SensorUpdateEventArgs { Message = sensorValues[i]});
          return Json("ding");
        } catch(Exception e)
        {
          _logger.LogError(e.Message);
          return Json("ding");
        }
      }
    }
  }
}
