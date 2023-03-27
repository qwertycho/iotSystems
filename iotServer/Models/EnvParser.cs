using Newtonsoft.Json;
using MySqlConnector;

namespace iotServer.classes
{

    public class dbSettings
    {
        public string? server { get; set; }
        public string? database { get; set; }
        public string? user { get; set; }
        public string? password { get; set; }
    }

    /// <summary>
    /// This class is used to parse the env.xml file
    /// </summary>
    public class EnvParser
    {
        private class EnvSettings
        {
            public dbSettings? dbSettings { get; set; }
        }
        /// <summary>
        /// Returns the dbSettings object with the values from the env.xml file
        /// </summary>
        /// <returns>dbSettings</returns>
        public static dbSettings getDbSettings()
        {
            string file = System.IO.File.ReadAllText("data/env.json");
            // convert the dbsettings json to a dbsettings object
            EnvSettings? settings = JsonConvert.DeserializeObject<EnvSettings>(file);
            return settings.dbSettings;
        }

        /// <summary>
        /// Returns a MySqlConnectionStringBuilder object with the values from the EnvParser class
        /// </summary>
        /// <returns>MySqlConnectionStringBuilder</returns>
        public static MySqlConnectionStringBuilder ConnectionStringBuilder()
        {
            dbSettings settings = getDbSettings();

            var builder = new MySqlConnectionStringBuilder
            {
                Server = settings.server,
                UserID = settings.user,
                Password = settings.password,
                Database = settings.database,
            };

            return builder;
        }
    }
}