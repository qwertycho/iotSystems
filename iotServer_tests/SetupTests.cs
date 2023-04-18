using iotServer.Controllers;
using Microsoft.AspNetCore.Mvc;
using iotServer.classes;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace iotServer_tests
{
    [TestClass]
    public class SetupTests
    {
        [TestMethod]
        public void TestGenerateSetupFromForm()
        {
            SetupModel setupModel = new SetupModel();

            IFormCollection form = new FormCollection(new Dictionary<string, StringValues>()
            {
                {"id", "1"},
                {"deviceID", "1"},
                {"aanTijd", "0600"},
                {"uitTijd", "2200"},
                {"maxTemp", "26,7"},
                {"minTemp", "18.3"}
            });

            DeviceSetup setup = setupModel.generateSetupFromForm(form);

            Assert.AreEqual(1, setup.id);
            Assert.AreEqual(1, setup.deviceID);
            Assert.AreEqual(600, setup.aanTijd);
            Assert.AreEqual(2200, setup.uitTijd);
            Assert.AreEqual(26.7, setup.maxTemp, 0.1);
            Assert.AreEqual(18.3, setup.minTemp, 0.1);
        }

        [TestMethod]
        public void TestGenerateSetupFromFormThrowsOnDeviceID()
        {
            SetupModel setupModel = new SetupModel();

            IFormCollection form = new FormCollection(new Dictionary<string, StringValues>()
            {
                {"id", "1"},
                {"deviceID", "0"},
                {"aanTijd", "0600"},
                {"uitTijd", "2200"},
                {"maxTemp", "26.7"},
                {"minTemp", "18,3"}
            });

            Assert.ThrowsException<Exception>(() => setupModel.generateSetupFromForm(form));
        }

        [TestMethod]
        public void TestGenerateSetupFromFormThrowsOnEmpty()
        {
            SetupModel setupModel = new SetupModel();

            IFormCollection form = new FormCollection(new Dictionary<string, StringValues>()
            {
                {"id", "1"},
                {"deviceID", "1"},
            });

            Assert.ThrowsException<Exception>(() => setupModel.generateSetupFromForm(form));
        }

    }
}