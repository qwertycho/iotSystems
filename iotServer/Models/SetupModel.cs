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

        public async Task<DeviceSetup> GetDeviceSetupAsync(int id)
        {
            using var connection = new MySqlConnection(builder.ConnectionString);
            await connection.OpenAsync();

            using var command = new MySqlCommand("SELECT setupID, aanTijd, uitTijd, maxTemp, minTemp FROM devices INNER JOIN setup ON devices.setup = setup.setupID WHERE devices.deviceID = @id", connection);
            command.Parameters.AddWithValue("@id", id);

            using var reader = await command.ExecuteReaderAsync();

            DeviceSetup setup = new DeviceSetup();


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

// updaten zodat id van device word gebruikt met een inner join
        public async Task SaveSetupAsync(DeviceSetup setup)
        {
            using var connection = new MySqlConnection(builder.ConnectionString);
            await connection.OpenAsync();

            using var command = new MySqlCommand(@"
            IF NOT EXISTS (SELECT * FROM setup WHERE setupID = @id) 
            THEN
                INSERT INTO setup (aanTijd, uitTijd, maxTemp, minTemp)
                VALUES (@aanTijd, @uitTijd, @maxTemp, @minTemp);

                UPDATE devices
                SET setup = 1
                WHERE deviceID = @id;

            ELSE
                UPDATE setup
                SET aanTijd = @aanTijd, 
                uitTijd = @uitTijd, 
                maxTemp = @maxTemp, 
                minTemp = @minTemp
                WHERE setupID = @id;
            END IF
            ", connection);

            command.Parameters.AddWithValue("@id", setup.id);
            command.Parameters.AddWithValue("@aanTijd", setup.aanTijd);
            command.Parameters.AddWithValue("@uitTijd", setup.uitTijd);
            command.Parameters.AddWithValue("@maxTemp", setup.maxTemp);
            command.Parameters.AddWithValue("@minTemp", setup.minTemp);

            await command.ExecuteNonQueryAsync();

            connection.Close();
        }

    }
}