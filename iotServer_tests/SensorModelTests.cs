using iotServer.classes;

namespace iotServer_tests
{
  [TestClass]
  public class SensorModelTests
  {
    
    SensorModel model = new SensorModel();


    [TestMethod]
    public void TestParseTempCorrect()
    {
      SensorValue testVal = new SensorValue();
        testVal.id = 1;
        testVal.type = "temp";
        testVal.value = "1.234";

      Assert.AreEqual(1.234, model.ParseTemp(testVal), 0.1); 
      testVal.value = "1,234";
      Assert.AreEqual(1.234, model.ParseTemp(testVal), 0.1); 
    }

    [TestMethod]
    public void TestParseTempThrowsTypeError()
    {
      SensorValue testVal = new SensorValue();
      testVal.type = "Geen Temp lol";
      Assert.ThrowsException<Exception>(() => model.ParseTemp(testVal));
    }
  }
}
