using Microsoft.AspNetCore.Diagnostics;
using iotServer.classes;
using iotServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace iotServer.Controllers
{
   public class SensorController : Controller
  {
    private readonly ILogger<SensorController> _logger;
    private readonly SensorModel _sensorModel;
    private NewsLetter.NewsLetter  _newsLetter;
    private readonly DeviceModel _deviceModel;

    public SensorController(ILogger<SensorController> logger, NewsLetter.NewsLetter newsLetter)
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
         _logger.LogError(data.id.ToString()); 
         _logger.LogError(data.value.ToString());
         _logger.LogError(data.type.ToString());
        SensorResponse res = new SensorResponse();

        float temp = _sensorModel.ParseTemp(data);
        int sensorID = await _sensorModel.GetSensorID(data);
        res.hasSetupChanged = await _deviceModel.HasSetupChanged(data.id);
        
        await _sensorModel.InsertTemp(sensorID, temp);

        _newsLetter.OnSensorUpdate(new NewsLetter.SensorUpdateEventArgs {value = data.value, deviceID = data.id, sensor = data.type});

        return Json(res);
      } catch(Exception e)
      {
        _logger.LogError(e.Message);
        SensorResponse res = new SensorResponse();
        res.message = e.Message;
        res.statusCode = 500;
        res.hasSetupChanged = false;
        return Json(res);
      }
    }
  }
}
