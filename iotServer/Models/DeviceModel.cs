using Newtonsoft.Json;
using MySqlConnector;

namespace iotServer.classes{

    public class DeviceModel
    {
        public async  Task<List<Device>> getAllDevicesAsync()
        {
           var builder = EnvParser.ConnectionStringBuilder();

            using var connection = new MySqlConnection(builder.ConnectionString);
            await connection.OpenAsync();

            using var cmd = new MySqlCommand
            {
                Connection = connection,
                CommandText = "SELECT deviceID, deviceNaam, groepID, uuid, aanmeldDatum FROM devices WHERE actief = 1",
            };

            using var reader = await cmd.ExecuteReaderAsync();

            List<Device> devices = new List<Device>();

            while (await reader.ReadAsync())
            {

                List<string> sensors = await getDeviceSensors(reader.GetInt32(0));

                Device device = new Device{
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Group = reader.GetInt32(2),
                    Uuid = reader.GetString(3),
                    Date = reader.GetDateTime(4),
                    Sensors = sensors
                };

                devices.Add(device);
            }
            return devices;
        }

        private async Task<List<string>> getDeviceSensors(int id)
        {
            var builder = EnvParser.ConnectionStringBuilder();

            using var connection = new MySqlConnection(builder.ConnectionString);
            await connection.OpenAsync();

            using var cmd = new MySqlCommand
            {
                Connection = connection,
                CommandText = "SELECT sensorName FROM sensors WHERE deviceID = @id",
            };

            cmd.Parameters.AddWithValue("@id", id);

            using var reader = await cmd.ExecuteReaderAsync();

            List<string> sensors = new List<string>();

            while (await reader.ReadAsync())
            {
                sensors.Add(reader.GetString(0));
            }

            return sensors;
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
            public int? Group { get; set; }
            public string Uuid { get; set; }
            public DateTime Date { get; set; }
            public List<string>? Sensors { get; set; }
        }
    }
}