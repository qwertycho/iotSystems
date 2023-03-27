using Newtonsoft.Json;
using MySqlConnector;

namespace iotServer.classes
{

    public class DeviceModel
    {
        public async Task<List<Device>> getAllDevicesAsync()
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

                Device device = new Device
                {
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

        public async Task<List<Device>> getDeviceByGroupAsync(int groupID)
        {
            var builder = EnvParser.ConnectionStringBuilder();
            using var connection = new MySqlConnection(builder.ConnectionString);
            await connection.OpenAsync();

            using var cmd = new MySqlCommand
            {
                Connection = connection,
                CommandText = "SELECT deviceID, deviceNaam, groepID, uuid, aanmeldDatum FROM devices WHERE actief = 1 AND groepID = @groupID",
            };

            cmd.Parameters.AddWithValue("@groupID", groupID);

            using var reader = await cmd.ExecuteReaderAsync();

            List<Device> devices = new List<Device>();

            while (await reader.ReadAsync())
            {

                List<string> sensors = await getDeviceSensors(reader.GetInt32(0));

                Device device = new Device
                {
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

        public async Task<List<Group>> GetAllGroupsAsync()
        {
            var builder = EnvParser.ConnectionStringBuilder();

            using var connection = new MySqlConnection(builder.ConnectionString);

            await connection.OpenAsync();


            using var cmd = new MySqlCommand
            {
                Connection = connection,
                CommandText = "SELECT groepID, groepNaam, groepKleur FROM groepen WHERE actief = 1",
            };

            using var reader = await cmd.ExecuteReaderAsync();

            List<Group> groups = new List<Group>();

            while (await reader.ReadAsync())
            {
                Group group = new Group
                {
                    id = reader.GetInt32(0),
                    name = reader.GetString(1),
                    color = reader.GetString(2),
                    devices = getDeviceByGroupAsync(reader.GetInt32(0)).Result
                };

                groups.Add(group);
            }


            return groups;
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
}