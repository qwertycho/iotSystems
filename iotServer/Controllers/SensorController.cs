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
        
        SensorResponse res = new SensorResponse();

        float temp = _sensorModel.ParseTemp(data);
        int sensorID = await _sensorModel.GetSensorID(data);
        res.hasSetupChanged = await _deviceModel.HasSetupChanged(data.id);
        
        await _sensorModel.InsertTemp(sensorID, temp);

        //_newsLetter.ding()

        return Json(res);
      } catch(Exception e)
      {

        SensorResponse res = new SensorResponse();
        res.message = e.Message;
        res.statusCode = 500;
        res.hasSetupChanged = false;
        return Json(res);
      }
    }
  }
}
