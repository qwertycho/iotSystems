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
           dbSettings settings = new EnvParser().getDbSettings();
            Assert.IsInstanceOfType(settings, typeof(dbSettings));
        }

        [TestMethod]
        public void TestGetDBSettings()
        {
           dbSettings settings = new EnvParser().getDbSettings();
            Assert.IsNotNull(settings.database);
            Assert.IsNotNull(settings.password);
            Assert.IsNotNull(settings.server);
            Assert.IsNotNull(settings.user);
        }
    }
}