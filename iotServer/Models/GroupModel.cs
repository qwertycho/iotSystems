using iotServer.classes;
using Newtonsoft.Json;
using MySqlConnector;

namespace iotServer.Models
{
    public class GroupModel
    {
      public GroupModel(){}

      
      public async Task<List<Group>> GetAllGroups()
      {

            var builder = EnvParser.ConnectionStringBuilder();
            using var connection = new MySqlConnection(builder.ConnectionString);
            await connection.OpenAsync();

            using var command = new MySqlCommand(@"
                SELECT * FROM groepen WHERE actief = 1
            ", connection);

            using var reader = await command.ExecuteReaderAsync();

            List<Group> groepen = new List<Group>();
            
            while(reader.Read())
            {
              Group group = new Group();
              group.id = reader.GetInt16(0);
              group.name = reader.GetString(1);
              group.color = reader.GetString(2);
              groepen.Add(group);
            }
            
            connection.Close();
            return groepen;
      }

      public async Task Update(int id, string color, string name)
      {
            var builder = EnvParser.ConnectionStringBuilder();
            using var connection = new MySqlConnection(builder.ConnectionString);
            await connection.OpenAsync();

            using var command = new MySqlCommand(@"
                UPDATE groepen SET groepNaam = @name, groepKleur = @color WHERE groepID = @id
            ", connection);

            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@color", color);
            command.Parameters.AddWithValue("id", id);

            await command.ExecuteNonQueryAsync();

            connection.Close();
      }

    public async Task newGroup(string name, string color)
    {
            var builder = EnvParser.ConnectionStringBuilder();
            using var connection = new MySqlConnection(builder.ConnectionString);
            await connection.OpenAsync();

            using var command = new MySqlCommand(@"
                INSERT INTO groepen (groepNaam, groepKleur) VALUES (@name, @color)
            ", connection);

            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@color", color);

            await command.ExecuteNonQueryAsync();

            connection.Close();
    }
    }
}
