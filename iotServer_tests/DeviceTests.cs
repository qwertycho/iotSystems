using iotServer.Controllers;
using Microsoft.AspNetCore.Mvc;
using iotServer.classes;

namespace iotServer_tests
{
    [TestClass]
    public class DeviceModelTests
    {
        [TestMethod]
        public void TestGetAllDevice()
        {
            DeviceModel testModel = new DeviceModel();
            Assert.IsInstanceOfType(testModel.getAllDevices(), typeof(List<DeviceModel.Device>));
            Assert.IsNotNull(testModel.getAllDevices());
        }

        [TestMethod]
        public void TestGetDeviceValue()
        {
            DeviceModel testModel = new DeviceModel();
            Assert.IsInstanceOfType(testModel.getDeviceValue(1), typeof(DeviceModel.SensorValue));
            Assert.IsNotNull(testModel.getDeviceValue(1));
        }
    }
}