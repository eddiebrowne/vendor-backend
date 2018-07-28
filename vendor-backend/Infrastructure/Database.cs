using System.Collections.Generic;
using System.Data;
using Domain;
using Microsoft.Data.Sqlite;

namespace Infrastructure
{
  public class Database : IDatabase
  {
    private static SqliteConnection _connection;

    public int AddProduct(IProduct product)
    {
      using (_connection = new SqliteConnection(Config.ConnectionString))
      {
        var command = _connection.CreateCommand();
        command.CommandText =
          "INSERT INTO tProduct(Name, Quantity, Price, UnitID) VALUES(@Name, @Quantity, @Price, @UnitID)";
        command.Parameters.Add(new SqliteParameter("Name", SqliteType.Text) {Value = product.Name});
        command.Parameters.Add(new SqliteParameter("Quantity", SqliteType.Integer) {Value = product.Quantity});
        command.Parameters.Add(new SqliteParameter("Price", SqliteType.Real) {Value = product.Price});
        command.Parameters.Add(new SqliteParameter("UnitID", SqliteType.Integer) {Value = product.UnitType});

        _connection.Open();
        var result = command.ExecuteNonQuery();
        _connection.Close();
        return result;
      }
    }

    public IProduct GetProduct(int id)
    {
      using (_connection = new SqliteConnection(Config.ConnectionString))
      {
        var command = _connection.CreateCommand();
        command.CommandText = $"SELECT * FROM tPRoduct WHERE ID = @ID";
        command.Parameters.Add(new SqliteParameter("ID", SqliteType.Integer) {Value = id});

        Product product = null;
        _connection.Open();
        using (var reader = command.ExecuteReader())
        {
          product = new Product()
          {
            Name = reader["Name"].ToString(),
            Price = decimal.Parse(reader["Price"].ToString()),
            Quantity = int.Parse(reader["Quantity"].ToString()),
            UnitType = (UnitType) byte.Parse(reader["UnitID"].ToString())
          };
        }
        _connection.Close();
        return product;
      }
    }

    public int RemoveProduct(int id)
    {
      using (_connection = new SqliteConnection(Config.ConnectionString))
      {
        var command = _connection.CreateCommand();
        command.CommandText = $"DELETE FROM tProduct WHERE ID = @ID";
        command.Parameters.Add(new SqliteParameter("ID", SqliteType.Integer) {Value = id});

        _connection.Open();
        var result = command.ExecuteNonQuery();
        _connection.Close();
        return result;
      }
    }

    public IEnumerable<IProduct> GetProducts()
    {
      using (_connection = new SqliteConnection(Config.ConnectionString))
      {
        var command = _connection.CreateCommand();
        command.CommandText = "SELECT * FROM tProduct";
        command.CommandType = CommandType.Text;

        var list = new List<Product>();
        _connection.Open();
        using (var reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            var product = new Product()
            {
              Name = reader["Name"].ToString(),
              Price = decimal.Parse(reader["Price"].ToString()),
              Quantity = int.Parse(reader["Quantity"].ToString()),
              UnitType = (UnitType) byte.Parse(reader["UnitID"].ToString())
            };
            list.Add(product);
          }
        }
        _connection.Close();
        return list;
      }
    }

    public static void RunScript(string script)
    {
      using(_connection = new SqliteConnection(Config.ConnectionString))
      {
        _connection.Open();
        using (var command = _connection.CreateCommand())
        {
          command.CommandText = script;
          command.ExecuteNonQuery();
        }
        _connection.Close();
      }
    }
  }
}