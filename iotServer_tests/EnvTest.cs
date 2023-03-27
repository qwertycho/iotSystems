using iotServer.Controllers;
using Microsoft.AspNetCore.Mvc;
using iotServer.classes;

namespace iotServer_tests
{
    [TestClass]
    public class envParserTests
    {
        [TestMethod]
        public void TestGetDBSettingsType()
        {
            Assert.IsInstanceOfType(EnvParser.getDbSettings(), typeof(dbSettings));
        }

        [TestMethod]
        public void TestGetDBSettings()
        {
           dbSettings settings = EnvParser.getDbSettings();
            Assert.IsNotNull(settings.database);
            Assert.IsNotNull(settings.password);
            Assert.IsNotNull(settings.server);
            Assert.IsNotNull(settings.user);
        }
    }
}