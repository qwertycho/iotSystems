using MySqlConnector;

namespace iotServer.classes
{
  public class SensorModel
  {
    public SensorModel() {}

    public async Task<int> GetSensorID(SensorValue data)
    {
      var builder = EnvParser.ConnectionStringBuilder();
      using var connection = new MySqlConnection(builder.ConnectionString);
      await connection.OpenAsync();

      using var cmd = new MySqlCommand
      {
        Connection = connection,
        CommandText = "SELECT sensorID FROM sensors WHERE deviceID = @id AND sensorName = @type",
      };

      cmd.Parameters.AddWithValue("@id", data.id);
      cmd.Parameters.AddWithValue("@type", data.type);

      using var reader = await cmd.ExecuteReaderAsync();

      int sensorID = 0;

      if(!reader.HasRows)
      {
        throw new Exception($"Sensor met deviceID {data.id} en type {data.type} bestaat niet!");
      }

      while( await reader.ReadAsync())
      {
        sensorID = reader.GetInt32(0);
      }
      return sensorID;
    }

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

    public async Task InsertTemp(int sensorID, float value)
    {
      var builder = EnvParser.ConnectionStringBuilder();
      using var connection = new MySqlConnection(builder.ConnectionString);
      await connection.OpenAsync();

      using var cmd = new MySqlCommand
      {
        Connection = connection,
        CommandText = "INSERT INTO tempMetingen (sensorID, tempWaarde) VALUES (@sensorID, @tempWaarde) ",
      };

      cmd.Parameters.AddWithValue("@sensorID", sensorID);
      cmd.Parameters.AddWithValue("@tempWaarde", value);

      await cmd.ExecuteNonQueryAsync();
    }
  }
}
