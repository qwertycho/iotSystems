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

            return setups;
        }

    }
}