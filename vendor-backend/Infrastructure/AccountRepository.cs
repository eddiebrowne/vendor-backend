using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using Domain;
using Microsoft.Data.Sqlite;

namespace Infrastructure
{
  public class AccountRepository : RepositoryBase, IAccountRepository
  {
    public AccountRepository(DatabaseSettings settings) : base(settings)
    {
    }
    
    public IAccount GetAccount(string email, string password)
    {
      IAccount account = null;
      var salt = GetSalt(email);

      if (!string.IsNullOrEmpty(salt))
      {
        var command = Connection.CreateCommand();
        command.CommandText = "SELECT Name, Email FROM tAccount WHERE Email = '@Email' AND PasswordHash = '@PasswordHash'";
        command.Parameters.Add(new SqliteParameter("Email", email));
        command.Parameters.Add(new SqliteParameter("PasswordHash", GetHash(password, salt)));

        account = ParseAccount(ExecuteReaderCommand(command));
      }

      return account;
    }

    private IAccount ParseAccount(IDataReader reader)
    {
      IAccount account;
      using (reader)
      {
        account = new Account()
        {
          Name = reader["Name"].ToString(),
          Email = reader["Email"].ToString()
        };
      }

      return account;
    }

    public IAccount GetAccountFromToken(string token)
    {
      var command = Connection.CreateCommand();
      command.CommandText = "SELECT Name, Email FROM tAccount WHERE Token = '@Token'";
      command.Parameters.Add(new SqliteParameter("Token", token));

      return ParseAccount(ExecuteReaderCommand(command));
    }

    public long Create(IAccount account)
    {
      byte[] saltBytes;
      var provider = new RNGCryptoServiceProvider();
      provider.GetBytes(saltBytes = new byte[16]);
      var salt = Encoding.ASCII.GetString(saltBytes);
      var command = Connection.CreateCommand();
      command.CommandText = "INSERT INTO tAccount (Name, Email, Salt, PasswordHash) VALUES (@Name, @Email, @Salt, @PasswordHash); SELECT CAST(last_insert_rowid() AS int)";
      command.Parameters.Add(new SqliteParameter("Name", account.Name));
      command.Parameters.Add(new SqliteParameter("Email", account.Email));
      command.Parameters.Add(new SqliteParameter("Salt", salt));
      command.Parameters.Add(new SqliteParameter("PasswordHash", GetHash(account.Password, salt)));

      return (long) ExecuteScalarCommand(command);
    }

    public void CreateDatabase(string accountName)
    {
      var database = $"{accountName}";
      if (!File.Exists(database))
      {
        SQLiteConnection.CreateFile(database);
        var script = File.ReadAllText(_databaseSettings.Scripts.Vendor);
        RunScript(script, $"Data Source={accountName}");
      }
    }

    private string GetHash(string password, string salt)
    {
      var provider = MD5.Create();
      return BitConverter.ToString(provider.ComputeHash(Encoding.ASCII.GetBytes(salt + password)));
    }

    public string GetSalt(string email)
    {
      var command = Connection.CreateCommand();
      command.CommandText = "SELECT Salt FROM tAccount WHERE Email = '@Email'";
      command.Parameters.Add(new SqliteParameter("Email", email));

      byte[] result;
      using (var reader = ExecuteReaderCommand(command))
      {
        var value = reader["Salt"];
        result = (byte[]) (value == DBNull.Value ? 0 : value);
      }
      
      return Encoding.Default.GetString(result);
    }
  }
}