using iotServer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace iotServer.classes
{
  public class SensorModel
  {


    public SensorModel() {}

    public float ParseTemp(SensorValue data)
    {
        AssertType(data.type, "temp");
        data.value = data.value.Replace(",", ".");
        return float.Parse(data.value);
    }

    private bool AssertType(string? checkType, string? toBeType)
    {
      if(checkType != toBeType)
      {
        throw new Exception($"{checkType} is niet van waarde: {toBeType}!");
      } return true;
    }

  
  }
}
