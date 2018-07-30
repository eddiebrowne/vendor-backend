using System.Collections.Generic;
using System.Data;
using Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
  public class ProductRepository : RepositoryBase, IProductRepository
  {
    public ProductRepository(DatabaseSettings settings) : base(settings)
    {
      
    }

    public int AddProduct(IProduct product)
    {
      var command = Connection.CreateCommand();
      command.CommandText =
        "INSERT INTO tProduct(Name, Quantity, Price, UnitID) VALUES(@Name, @Quantity, @Price, @UnitID)";
      command.Parameters.Add(new SqliteParameter("Name", SqliteType.Text) {Value = product.Name});
      command.Parameters.Add(new SqliteParameter("Quantity", SqliteType.Integer) {Value = product.Quantity});
      command.Parameters.Add(new SqliteParameter("Price", SqliteType.Real) {Value = product.Price});
      command.Parameters.Add(new SqliteParameter("UnitID", SqliteType.Integer) {Value = product.UnitType});
      return ExecuteNonQueryCommand(command);
    }

    public IProduct GetProduct(int id)
    {
      using (Connection)
      {
        var command = Connection.CreateCommand();
        command.CommandText = $"SELECT * FROM tPRoduct WHERE ID = @ID";
        command.Parameters.Add(new SqliteParameter("ID", SqliteType.Integer) {Value = id});

        Product product = null;
        using (var reader = ExecuteReaderCommand(command))
        {
          product = new Product()
          {
            Name = reader["Name"].ToString(),
            Price = decimal.Parse(reader["Price"].ToString()),
            Quantity = int.Parse(reader["Quantity"].ToString()),
            UnitType = (UnitType) byte.Parse(reader["UnitID"].ToString())
          };
        }

        return product;
      }
    }

    public int RemoveProduct(int id)
    {
      var command = Connection.CreateCommand();
      command.CommandText = $"DELETE FROM tProduct WHERE ID = @ID";
      command.Parameters.Add(new SqliteParameter("ID", SqliteType.Integer) {Value = id});
      return ExecuteNonQueryCommand(command);
    }

    public IEnumerable<IProduct> GetProducts()
    {
      using (Connection)
      {
        var command = Connection.CreateCommand();
        command.CommandText = "SELECT * FROM tProduct";
        command.CommandType = CommandType.Text;

        var list = new List<Product>();
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

        return list;
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
  }
}