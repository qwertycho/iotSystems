using Newtonsoft.Json;

using System.Xml;
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
        public dbSettings getDbSettings()
        {
            string file = System.IO.File.ReadAllText("data/env.json");
            // convert the dbsettings json to a dbsettings object
            EnvSettings? settings = JsonConvert.DeserializeObject<EnvSettings>(file);
            return settings.dbSettings;
        }
    }

}