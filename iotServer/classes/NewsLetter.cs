using iotServer.classes;

namespace iotServer.NewsLetter
{

    /// <summary>
    /// NewsLetter class, hiermee kan je events aanmaken en aanroepen. Het is de bedoeling dat je deze class injecteert in de controller waar je de events wilt gebruiken.
    /// </summary>
    public class NewsLetter
    {
        /// <summary>
        /// Voert de eventhandler uit voor sensorUpdate
        /// </summary>
        /// <param name="e">SensorUpdateEventArgs</param>
        /// <returns>void</returns>
        public virtual void OnSensorUpdate(SensorUpdateEventArgs e)
        {
            EventHandler<SensorUpdateEventArgs>? handler = sensorUpdate;
            Console.WriteLine("OnSensorUpdate");
            if (handler != null)
            {
                handler(null, e);
            }
        }

        /// <summary>
        /// Voert de eventhandler uit voor newDevice
        /// </summary>
        /// <param name="e">NewDeviceEventArgs</param>
        /// <returns>void</returns>
        /// <remarks>
        /// Deze methode ontvangt een NewDeviceEventArgs object, dat is hetzelfde als de NewDevice class in de iotServer.classes namespace.
        /// </remarks>
        public virtual void OnNewDevice(NewDeviceEventArgs e)
        {
            EventHandler<NewDeviceEventArgs>? handler = newDevice;
            Console.WriteLine("OnNewDevice");
            if (handler != null)
            {
                handler(null, e);
            }
        }

        /// <summary>
        /// Eventhandler voor sensorUpdate, hiermee kan je een eventhandler toevoegen aan de sensorUpdate event
        /// </summary>
        public event EventHandler<SensorUpdateEventArgs>? sensorUpdate;

        /// <summary>
        /// Eventhandler voor newDevice, hiermee kan je een eventhandler toevoegen aan de newDevice event
        /// </summary>
        public event EventHandler<NewDeviceEventArgs>? newDevice;

    }

    public class NewDeviceEventArgs : EventArgs
    {
        public string? Uuid { get; set; }
        public List<string>? Sensors { get; set; }
        public DeviceSetup? Setup { get; set; }
    }

    public class SensorUpdateEventArgs : EventArgs
    {
      public string eventType {get; set;}
      public int deviceID {get; set;}
      public string? value {get; set;}
      public string? sensor {get; set;}
    }

}
