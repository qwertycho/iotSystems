namespace iotServer.classes
{
    public class NewDevice
    {
        public string? Uuid { get; set; }
        public List<string>? Sensors { get; set; }
        public DeviceSetup? Setup { get; set; }
    }

    public class DeviceSetup
    {
        public int id { get; set; }
        public float maxTemp { get; set; }
        public float minTemp { get; set; }
        public int aanTijd { get; set; }
        public int uitTijd { get; set; }
    }

    public class Group
    {
        public int id { get; set; }
        public string? name { get; set; }
        public string? color { get; set; }
        public List<Device>? devices { get; set; }
    }

    public class SensorValue
    {
        public int id { get; set; }
        public int value { get; set; }
    }

    public class Device
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Group { get; set; }
        public string? Uuid { get; set; }
        public DateTime Date { get; set; }
        public List<string>? Sensors { get; set; }
    }
}