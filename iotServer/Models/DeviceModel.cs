using Newtonsoft.Json;
using MySqlConnector;

namespace iotServer.classes{

    public class DeviceModel
    {
        public async void dbTest2()
        {
           dbSettings settings = new EnvParser().getDbSettings();

            var builder = new MySqlConnectionStringBuilder
            {
                Server = settings.server,
                UserID = settings.user,
                Password = settings.password,
                Database = settings.database,
            };

            using var connection = new MySqlConnection(builder.ConnectionString);
            await connection.OpenAsync();

            using var cmd = new MySqlCommand
            {
                Connection = connection,
                CommandText = "SELECT * FROM devices",
            };

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                Console.WriteLine(reader.GetInt16(0));
                Console.WriteLine(reader.GetString(2));
            }
        }

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
            public string? Name { get; set; }
            public string? Group { get; set; }
            public List<string>? Sensors { get; set; }
        }
    }
}