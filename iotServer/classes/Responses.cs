namespace iotServer.classes
{
  public class SensorResponse
  {
    public int statusCode {get; set;}
    public bool hasSetupChanged {get; set;}
    public string? message {get; set;}
  }
}
