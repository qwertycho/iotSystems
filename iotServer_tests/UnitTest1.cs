using iotServer.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace iotServer_tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            HomeController1 testController = new HomeController1();
            Assert.AreEqual("Hello World!", testController.testFunc());
        }
    }
}