using Newtonsoft.Json;
using MySqlConnector;

namespace iotServer.classes
{
    public class DeviceModel
    {
        private List<NewDevice> devicesToInit = new List<NewDevice>();

        /// <summary>
        /// Controleerd of alle gegevens van een NewDevice zijn ingevuld
        /// </summary>
        /// <param name="device">NewDevice</param>
        /// <exception cref="Exception">Geen uuid of sensors gegeven</exception>
        public void validateNewDevice(NewDevice device)
        {
            if (device.Uuid == null || device.Uuid == "")
            {
                throw new Exception("No uuid given");
            }

            if (device.Sensors == null || device.Sensors.Count == 0 || device.Sensors[0] == "")
            {
                throw new Exception("No sensors given");
            }
        }

        public async Task<List<Device>> getAllDevicesAsync()
        {
            var builder = EnvParser.ConnectionStringBuilder();
            using var connection = new MySqlConnection(builder.ConnectionString);
            await connection.OpenAsync();

            using var cmd = new MySqlCommand
            {
                Connection = connection,
                CommandText = "SELECT deviceID, deviceNaam, groepID, uuid, aanmeldDatum FROM devices WHERE status = 'A'",
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
                CommandText = "SELECT deviceID, deviceNaam, groepID, uuid, aanmeldDatum FROM devices WHERE status = 'a' AND groepID = @groupID",
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

        public async Task<Device> GetDeviceByID(int id)
        {
            var builder = EnvParser.ConnectionStringBuilder();
            using var connection = new MySqlConnection(builder.ConnectionString);
            await connection.OpenAsync();

            using var cmd = new MySqlCommand
            {
                Connection = connection,
                CommandText = "SELECT deviceID, deviceNaam, groepID, uuid, aanmeldDatum FROM devices WHERE deviceID = @id",
            };

            cmd.Parameters.AddWithValue("@id", id);

            using var reader = await cmd.ExecuteReaderAsync();

            Device device = new Device();

            while (await reader.ReadAsync())
            {
                List<string> sensors = await getDeviceSensors(reader.GetInt32(0));

                device.Id = reader.GetInt32(0);
                device.Name = reader.GetString(1);
                device.Group = reader.GetInt32(2);
                device.Uuid = reader.GetString(3);
                device.Date = reader.GetDateTime(4);
                device.Sensors = sensors;
            }

            return device;
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

        public async Task<DeviceSetup> initDevice(NewDevice device)
        {

            validateNewDevice(device);

            // controleren of device al bestaat
            var builder = EnvParser.ConnectionStringBuilder();
            using var connection = new MySqlConnection(builder.ConnectionString);
            await connection.OpenAsync();

            using var cmd = new MySqlCommand
            {
                Connection = connection,
                CommandText = @"
                SELECT 
                deviceID
                from devices
                where uuid = @uuid
                ",
            };

            cmd.Parameters.AddWithValue("@uuid", device.Uuid);

            using var reader = await cmd.ExecuteReaderAsync();

            DeviceSetup deviceSetup = new DeviceSetup();

            if (reader.HasRows)
            {
                Console.WriteLine("Device found, loading setup");
                while (await reader.ReadAsync())
                {
                    deviceSetup = await GetDeviceSetup(reader.GetInt32(0));
                }
                return deviceSetup;
            }
            else
            {
                Console.WriteLine("Device not found, inserting new device");
                await insertDevice(device);
                deviceSetup.status = false;
                return deviceSetup;
            }
        }

        private async Task insertDevice(NewDevice device)
        {
            validateNewDevice(device);

            var builder = EnvParser.ConnectionStringBuilder();
            using var connection = new MySqlConnection(builder.ConnectionString);
            await connection.OpenAsync();

            using var cmd = new MySqlCommand
            {
                Connection = connection,
                CommandText = "INSERT INTO devices (uuid, status) VALUES (@uuid, 'N')",
            };

            cmd.Parameters.AddWithValue("@uuid", device.Uuid);

            await cmd.ExecuteNonQueryAsync();
            
            #pragma warning disable CS8604 // Deze hier omdat validateNewDevice errors throwt wanneer deze leeg zijn.
            await insertSensors(device.Sensors, device.Uuid);
            #pragma warning restore CS8604
            
        }

        public async Task<List<Device>> GetNewDevicesAsync()
        {
            var builder = EnvParser.ConnectionStringBuilder();
            using var connection = new MySqlConnection(builder.ConnectionString);
            await connection.OpenAsync();

            using var cmd = new MySqlCommand
            {
                Connection = connection,
                CommandText = "SELECT deviceID, deviceNaam, groepID, uuid, aanmeldDatum FROM devices WHERE status = 'N'",
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

        private async Task<Device> GetDeviceByUuidAsync(string uuid)
        {
            var builder = EnvParser.ConnectionStringBuilder();
            using var connection = new MySqlConnection(builder.ConnectionString);
            await connection.OpenAsync();

            using var cmd = new MySqlCommand
            {
                Connection = connection,
                CommandText = "SELECT deviceID, deviceNaam, groepID, uuid, aanmeldDatum FROM devices WHERE uuid = @uuid",
            };

            cmd.Parameters.AddWithValue("@uuid", uuid);

            using var reader = await cmd.ExecuteReaderAsync();

            Device device = new Device();

            while (await reader.ReadAsync())
            {
                device.Id = reader.GetInt32(0);
                device.Name = reader.GetString(1);
                device.Group = reader.GetInt32(2);
                device.Uuid = reader.GetString(3);
                device.Date = reader.GetDateTime(4);
            }
            return device;
        }

        private async Task insertSensors(List<string> sensors, string uuid)
        {
            var builder = EnvParser.ConnectionStringBuilder();
            using var connection = new MySqlConnection(builder.ConnectionString);
            await connection.OpenAsync();

            Device device = await GetDeviceByUuidAsync(uuid);

            foreach (string sensor in sensors)
            {
                using var cmd = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = "INSERT INTO sensors (sensorName, deviceID) VALUES (@name, @deviceID)",
                };

                cmd.Parameters.AddWithValue("@name", sensor);
                cmd.Parameters.AddWithValue("@deviceID", device.Id);

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<DeviceSetup> GetDeviceSetup(int id)
        {
            var builder = EnvParser.ConnectionStringBuilder();
            using var connection = new MySqlConnection(builder.ConnectionString);
            await connection.OpenAsync();

            using var cmd = new MySqlCommand
            {
                Connection = connection,
                CommandText = "SELECT deviceID, maxTemp, minTemp, aanTijd, uitTijd FROM devices INNER JOIN setup ON devices.setup = setup.setupID WHERE deviceID = @id",
            };

            cmd.Parameters.AddWithValue("@id", id);

            using var reader = await cmd.ExecuteReaderAsync();

            DeviceSetup deviceSetup = new DeviceSetup();
            deviceSetup.status = false;

            while (await reader.ReadAsync())
            {
                deviceSetup.id = reader.GetInt32(0);
                deviceSetup.maxTemp = reader.GetFloat(1);
                deviceSetup.minTemp = reader.GetFloat(2);
                deviceSetup.aanTijd = reader.GetInt32(3);
                deviceSetup.uitTijd = reader.GetInt32(4);
                deviceSetup.status = true;
            }
            return deviceSetup;
        }

        public async Task<bool> HasSetupChanged(int id)
        {
            var builder = EnvParser.ConnectionStringBuilder();
            using var connection = new MySqlConnection(builder.ConnectionString);
            await connection.OpenAsync();

            using var cmd = new MySqlCommand
            {
                Connection = connection,
                CommandText = "SELECT * from devices WHERE deviceID = @id AND status = 'C' ",
            };

            cmd.Parameters.AddWithValue("@id", id);

            using var reader = await cmd.ExecuteReaderAsync();

            return reader.HasRows;
        }
    }
}
