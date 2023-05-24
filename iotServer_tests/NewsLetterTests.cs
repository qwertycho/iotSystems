
using iotServer.classes;
using iotServer.NewsLetter;

namespace iotServer_tests
{
    [TestClass]
    public class NewsLetterTests
    {
        [TestMethod]
        public void TestSensorUpdate()
        {
            NewsLetter newsLetter = new NewsLetter();

            bool eventFired = false;
            string? eventValue = "";
            string? eventSensor = "";
            int eventDeviceID = 0;

            newsLetter.sensorUpdate += (object? sender, SensorUpdateEventArgs e) =>
            {
                eventFired = true;
                eventValue = e.value;
                eventSensor = e.sensor;
                eventDeviceID = e.deviceID;
            };

            newsLetter.OnSensorUpdate(new SensorUpdateEventArgs()
            {
            deviceID = 1,
            value = "123",
            sensor = "sensor"
            });


            Assert.IsTrue(eventFired);
            Assert.AreEqual("123", eventValue);
            Assert.AreEqual(1, eventDeviceID);
            Assert.AreEqual("sensor", eventSensor);
        }

        [TestMethod]
        public void TestNewDeviceEvent()
        {
            NewsLetter newsLetter = new NewsLetter();

            bool eventFired = false;
            string? eventUuid = "";
            List<string>? eventSensors = new List<string>();
            DeviceSetup? eventSetup = new DeviceSetup();

            DeviceSetup Setup = new DeviceSetup()
            {
                id = 1,
                deviceID = 12345,
                maxTemp = 30,
                minTemp = 10,
                aanTijd = 10,
                uitTijd = 10,
                status = true,
                error = false
            };

            newsLetter.newDevice += (object? sender, NewDeviceEventArgs e) =>
            {
                eventFired = true;
                eventUuid = e.Uuid;
                eventSensors = e.Sensors;
                eventSetup = e.Setup;
            };

            newsLetter.OnNewDevice(new NewDeviceEventArgs()
            {
                Uuid = "12345",
                Sensors = new List<string>() { "temp", "hum" },
                Setup = Setup
            });

            Assert.IsTrue(eventFired);
            Assert.AreEqual("12345", eventUuid);
            Assert.AreEqual("hum", eventSensors[1]);
            Assert.AreEqual("temp", eventSensors[0]);
            Assert.AreEqual(Setup, eventSetup);

        }

    }
}
