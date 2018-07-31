using System;
using System.Data;
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
        command.CommandText = "SELECT Name, Email FROM tAccount WHERE Email = @0 AND PasswordHash = @1";
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
      command.CommandText = "SELECT Name, Email FROM tAccount WHERE Token = @0";
      command.Parameters.Add(new SqliteParameter("Token", token));

      return ParseAccount(ExecuteReaderCommand(command));
    }

    private string GetHash(string password, string salt)
    {
      var provider = MD5.Create();
      return BitConverter.ToString(provider.ComputeHash(Encoding.ASCII.GetBytes(salt + password)));
    }

    public string GetSalt(string email)
    {
      var command = Connection.CreateCommand();
      command.CommandText = "SELECT Salt FROM tAccount WHERE Email = @0";
      command.Parameters.Add(new SqliteParameter("Email", email));

      return (string) ExecuteScalarCommand(command);
    }
  }
}