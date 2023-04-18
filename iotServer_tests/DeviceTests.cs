using iotServer.Controllers;
using Microsoft.AspNetCore.Mvc;
using iotServer.classes;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace iotServer_tests
{
    [TestClass]
    public class DeviceTests
    {
        [TestMethod]
        public void TestValidateNewDeviceValid()
        {
            DeviceModel deviceModel = new DeviceModel();

            NewDevice device = new NewDevice();
            device.Uuid = "12345";
            device.Sensors = new List<String>() { "temp", "hum" };

            deviceModel.validateNewDevice(device);

        }

        [TestMethod]
        public void TestValidateNewDeviceThrowsOnEmpty()
        {
            DeviceModel deviceModel = new DeviceModel();

            NewDevice device = new NewDevice();

            Assert.ThrowsException<Exception>(() => deviceModel.validateNewDevice(device));
        }

    }
}