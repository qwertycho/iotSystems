using Newtonsoft.Json;

namespace iotServer.classes{

    public class DeviceModel
    {

        public List<Device> getAllDevices(){
            List<Device>? devices = new List<Device>();

            string file = System.IO.File.ReadAllText("data/devices.json");

            devices = JsonConvert.DeserializeObject<List<Device>>(file);

            return devices;
        }

        public SensorValue getDeviceValue(int id){
            Random random = new Random();

            int value = random.Next(1, 11);

            SensorValue res = new SensorValue{
                id = id,
                value = value
            };

            return res;
        }

        public class SensorValue
        {
            public int id { get; set; }
            public int value { get; set; }
        }

        public class Device
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Group { get; set; }
            public List<string> Sensors { get; set; }
        }
    }
}