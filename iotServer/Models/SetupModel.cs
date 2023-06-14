using MySqlConnector;

namespace iotServer.classes
{
    public class SetupModel
    {

        private MySqlConnectionStringBuilder builder;

        public SetupModel()
        {
            builder = EnvParser.ConnectionStringBuilder();
        }

        /// <summary>
        /// Maakt een setup object aan
        /// </summary>
        /// <param name="setup">
        /// IFomCollection (Ruest.Form)
        ///</param>
        /// <returns>
        /// DeviceSetup
        ///</returns>
        /// <exception cref="Exception">
        /// Form item(s) is leeg
        ///</exception>
        public DeviceSetup generateSetupFromForm(IFormCollection form)
        {
            DeviceSetup setup = new DeviceSetup();
            setup.id = Convert.ToInt32(form["id"]);
            setup.deviceID = Convert.ToInt32(form["deviceID"]);

            if(form.Count < 4)
            {
                throw new Exception("Form item(s) is leeg");
            }

            foreach(var item in form)
            {
                if(item.Value == "" || item.Value == "")
                {
                    throw new Exception($"Value {item.Key} is empty");
                }
            }

            if (setup.deviceID == 0)
            {
                throw new Exception("DeviceID is 0");
            }

            setup.aanTijd = Convert.ToInt32(form["aanTijd"]);
            setup.uitTijd = Convert.ToInt32(form["uitTijd"]);

            String? maxTemp = form["maxTemp"];
            String? minTemp = form["minTemp"];

            #pragma warning disable CS8602
            maxTemp = maxTemp.Replace(",", ".");
            minTemp = minTemp.Replace(",", ".");

            setup.maxTemp = float.Parse(maxTemp);
            setup.minTemp = float.Parse(minTemp);
            #pragma warning restore CS8602

            return setup;
        }

        public async Task<DeviceSetup> GetDeviceSetupAsync(int id)
        {
            using var connection = new MySqlConnection(builder.ConnectionString);
            await connection.OpenAsync();

            using var command = new MySqlCommand("SELECT setupID, aanTijd, uitTijd, maxTemp, minTemp FROM devices INNER JOIN setup ON devices.setup = setup.setupID WHERE devices.deviceID = @id", connection);
            command.Parameters.AddWithValue("@id", id);

            using var reader = await command.ExecuteReaderAsync();

            DeviceSetup setup = new DeviceSetup();
            setup.deviceID = id;


            if (!reader.HasRows)
            {
                connection.Close();
                return setup;
            }
            else
            {

                while (await reader.ReadAsync())
                {
                    setup.id = reader.GetInt32(0);
                    setup.aanTijd = reader.GetInt32(1);
                    setup.uitTijd = reader.GetInt32(2);
                    setup.maxTemp = reader.GetFloat(3);
                    setup.minTemp = reader.GetFloat(4);
                }
            }

            connection.Close();

            return setup;
        }

        /// <summary>
        /// Haal alle setups op uit de database
        /// </summary>
        /// <returns>
        /// List<DeviceSetup>
        ///</returns>
        public async Task<List<DeviceSetup>> GetSetupsAsync()
        {
            using var connection = new MySqlConnection(builder.ConnectionString);
            await connection.OpenAsync();

            using var command = new MySqlCommand("SELECT * FROM setup", connection);

            using var reader = await command.ExecuteReaderAsync();

            List<DeviceSetup> setups = new List<DeviceSetup>();

            while (await reader.ReadAsync())
            {
                DeviceSetup setup = new DeviceSetup();
                setup.id = reader.GetInt32(0);
                setup.aanTijd = reader.GetInt32(1);
                setup.uitTijd = reader.GetInt32(2);
                setup.maxTemp = reader.GetFloat(3);
                setup.minTemp = reader.GetFloat(4);

                setups.Add(setup);
            }

            connection.Close();

            return setups;
        }

        public async Task SaveSetupAsync(DeviceSetup setup)
        {
            using var connection = new MySqlConnection(builder.ConnectionString);
            await connection.OpenAsync();

            using var command = new MySqlCommand(@"
            IF NOT EXISTS (SELECT setupID FROM devices INNER JOIN setup ON devices.setup = setup.setupID WHERE devices.deviceID = @DEVICEID)
            THEN
                INSERT INTO setup (aanTijd, uitTijd, maxTemp, minTemp)
                VALUES (@aanTijd, @uitTijd, @maxTemp, @minTemp);

                UPDATE devices
                SET setup = LAST_INSERT_ID()
                WHERE deviceID = @DEVICEID;
           

            ELSE
                UPDATE setup
                SET aanTijd = @aanTijd, 
                uitTijd = @uitTijd, 
                maxTemp = @maxTemp, 
                minTemp = @minTemp
                WHERE setupID = @id;
            END IF;
            UPDATE devices SET status = 'C' WHERE deviceID = @id
            ", connection);

            command.Parameters.AddWithValue("@id", setup.id);
            command.Parameters.AddWithValue("@aanTijd", setup.aanTijd);
            command.Parameters.AddWithValue("@uitTijd", setup.uitTijd);
            command.Parameters.AddWithValue("@maxTemp", setup.maxTemp);
            command.Parameters.AddWithValue("@minTemp", setup.minTemp);
            command.Parameters.AddWithValue("@DEVICEID", setup.deviceID);

            await command.ExecuteNonQueryAsync();
            
            connection.Close();
        }
    }
}
