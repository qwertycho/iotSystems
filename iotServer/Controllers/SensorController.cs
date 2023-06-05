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
    private readonly DeviceModel _deviceModel;

    public SensorController(ILogger logger, NewsLetter.NewsLetter newsLetter)
    {
      _logger = logger;
      _sensorModel = new SensorModel();
      _newsLetter = newsLetter;
      _deviceModel = new DeviceModel();
    }

    public async Task<JsonResult> temp([FromBody] SensorValue data)
    {
      try
      {
        float temp = _sensorModel.ParseTemp(data);
    //    int sensorID = await _sensorModel.GetSensorID(data);
        bool SetupChanged = await _deviceModel.HasSetupChanged(data.id);
        return Json("jup");
      } catch(Exception e)
      {
        return Json("nope");
      }
    }
  }
}
