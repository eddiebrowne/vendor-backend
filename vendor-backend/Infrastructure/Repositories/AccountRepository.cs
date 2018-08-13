using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Domain;
using Domain.Services;

namespace Infrastructure.Repositories
{
  public class AccountRepository : RepositoryBase, IAccountRepository
  {
    public AccountRepository(DatabaseSettings settings) : base(settings)
    {
    }

    public IAccount GetAccount(string email, string password)
    {
      var salt = GetSalt(email);

      if (!string.IsNullOrEmpty(salt))
      {
        var hash = GetHash(password, salt);
        using (var command = Connection.CreateCommand())
        {
          command.CommandText =
            "SELECT [Name], [Email] FROM tAccount WHERE [Email] = @Email AND PasswordHash = @PasswordHash";
          command.Parameters.AddWithValue("Email", email);
          command.Parameters.AddWithValue("PasswordHash", hash);

          return ParseAccount(ExecuteReaderCommand(command));
        }
      }

      return null;
    }

    private IAccount ParseAccount(IDataReader reader)
    {
      IAccount account = null;
      using (reader)
      {
        while (reader.Read())
        {
          account = new Account()
          {
            Name = reader["Name"].ToString(),
            Email = reader["Email"].ToString()
          };
        }
      }

      return account;
    }

    public IAccount GetAccountFromToken(string token)
    {
      using (var command = Connection.CreateCommand())
      {
        command.CommandText = "SELECT Name, Email FROM tAccount WHERE Token = @Token";
        command.Parameters.AddWithValue("Token", token);

        return ParseAccount(ExecuteReaderCommand(command));
      }
    }

    public int Create(IAccount account)
    {
      byte[] saltBytes;
      var provider = new RNGCryptoServiceProvider();
      provider.GetBytes(saltBytes = new byte[16]);
      var salt = Encoding.ASCII.GetString(saltBytes);
      var hash = GetHash(account.Password, salt);

      using (var command = Connection.CreateCommand())
      {
        command.CommandText =
          "INSERT INTO tAccount (Name, [Email], Salt, PasswordHash) VALUES (@Name, @Email, @Salt, @PasswordHash); SELECT ID FROM tAccount WHERE rowid = (SELECT last_insert_rowid());";
        command.Parameters.AddWithValue("Name", account.Name);
        command.Parameters.AddWithValue("Email", account.Email);
        command.Parameters.AddWithValue("Salt", salt);
        command.Parameters.AddWithValue("PasswordHash", hash);

        return (int) (long) ExecuteScalarCommand(command);
      }
    }

    public void CreateDatabase(string accountName)
    {
      var database = $"{accountName}";
      if (!File.Exists(database))
      {
        SQLiteConnection.CreateFile(database);
        var script = File.ReadAllText(DatabaseSettings.Scripts.Vendor);
        RunScript(script, $"Data Source={accountName}");
      }
    }

    public void StoreToken(IAccount account, string token)
    {
      using (var command = Connection.CreateCommand())
      {
        command.CommandText = "UPDATE tAccount SET Token = @Token WHERE Email = @Email";
        command.Parameters.AddWithValue("Email", account.Email);
        command.Parameters.AddWithValue("Token", token);
        ExecuteNonQueryCommand(command);
      }
    }

    public IVendor GetVendor(int vendorId)
    {
      var vendor = new Vendor();
      using (var command = MainConnection.CreateCommand())
      {
        command.CommandText = "SELECT Name FROM tAccount WHERE ID = @ID";
        command.Parameters.AddWithValue("ID", vendorId);
        vendor.Name = command.ExecuteScalar()?.ToString();
      }

      using (var command = MainConnection.CreateCommand())
      {
        command.CommandText =
          @"SELECT
              m.Name,
              m.Address,
              d.[Day],
              m.StartTime,
              m.EndTime
            FROM
              tMarket m
            INNER JOIN tVendorMarket vm ON
              vm.MarketID = m.ID
            INNER JOIN tDay d ON
              d.ID = m.DayOfWeek
            WHERE
              vm.AccountID = @ID";
        command.Parameters.AddWithValue("ID", vendorId);

        using (var reader = command.ExecuteReader())
        {
          vendor.Markets = new List<IMarket>();
          while (reader.Read())
          {
            var market = new Market();
            market.Name = reader.GetString(reader.GetOrdinal("Name"));
            market.Address = reader.GetString(reader.GetOrdinal("Address"));
            market.Day = reader.GetString(reader.GetOrdinal("Day"));
            market.StartTime = reader.GetString(reader.GetOrdinal("StartTime"));
            market.EndTime = reader.GetString(reader.GetOrdinal("EndTime"));
            vendor.Markets.Add(market);
          }
        }
      }

      return vendor;
    }

    private string GetHash(string password, string salt)
    {
      return BitConverter.ToString(MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(salt + password)));
    }

    private string GetSalt(string email)
    {
      using (var command = Connection.CreateCommand())
      {
        command.CommandText = "SELECT Salt FROM tAccount WHERE [Email] = @Email";
        command.Parameters.AddWithValue("Email", email);
        return ExecuteScalarCommand(command)?.ToString();
      }
    }
  }
}