using Microsoft.Data.Sqlite;

namespace Infrastructure
{
  public abstract class RepositoryBase
  {
    private static string _connectionString;

    protected RepositoryBase(DatabaseSettings settings)
    {
      _connectionString = settings.ConnectionString;
    }
    
    protected static SqliteConnection Connection
    {
      get
      {
        var connection = new SqliteConnection(_connectionString);
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
  }
}