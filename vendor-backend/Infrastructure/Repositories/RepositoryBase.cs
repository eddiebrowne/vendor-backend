using Microsoft.Data.Sqlite;

namespace Infrastructure.Repositories
{
  public abstract class RepositoryBase
  {
    protected static DatabaseSettings DatabaseSettings;
    
    protected RepositoryBase(DatabaseSettings settings)
    {
      DatabaseSettings = settings;
    }
    
    protected static SqliteConnection MainConnection
    {
      get
      {
        var connection = new SqliteConnection(DatabaseSettings.MainConnectionString);
        connection.Open();
        return connection;
      }
    }

    
    protected static SqliteConnection Connection
    {
      get
      {
        var connection = new SqliteConnection(DatabaseSettings.ConnectionString);
        connection.Open();
        return connection;
      }
    }
    
    public int ExecuteNonQueryCommand(SqliteCommand command)
    {
      using (Connection)
      {
        return command.ExecuteNonQuery();
      }
    }

    public object ExecuteScalarCommand(SqliteCommand command)
    {
      using (Connection)
      {
        return command.ExecuteScalar();
      }
    }

    public SqliteDataReader ExecuteReaderCommand(SqliteCommand command)
    {
      using (Connection)
      {
        return command.ExecuteReader();
      }
    }

    public static void RunScript(string script, string connectionString)
    {
      using (var connection = new SqliteConnection(connectionString))
      {
        using (var command = connection.CreateCommand())
        {
          command.CommandText = script;
          connection.Open();
          command.ExecuteNonQuery();
        }
      }
    }

    protected long GetId(string table)
    {
      var com = Connection.CreateCommand();
      com.CommandText = $"select seq from sqlite_sequence where name='{table}'; ";
      return (long) ExecuteScalarCommand(com);
    }
  }
}