using System.Collections.Generic;
using System.Data;
using System.IO;
using Domain;
using Microsoft.Data.Sqlite;

namespace Infrastructure.Repositories
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
        "INSERT INTO tProduct(Name, Quantity, Price, UnitID) VALUES(@Name, @Quantity, @Price, @UnitID); SELECT ID FROM tProduct WHERE rowid = (SELECT last_insert_rowid());";
      command.Parameters.AddWithValue("Name", product.Name);
      command.Parameters.AddWithValue("Quantity", product.Quantity);
      command.Parameters.AddWithValue("Price", product.Price);
      command.Parameters.AddWithValue("UnitID", product.UnitType);
      return (int) (long) ExecuteScalarCommand(command);
    }

    public IProduct GetProduct(int id)
    {
      using (Connection)
      {
        var command = Connection.CreateCommand();
        command.CommandText = $"SELECT * FROM tProduct WHERE ID = @ID";
        command.Parameters.Add(new SqliteParameter("ID", SqliteType.Integer) {Value = id});

        Product product = null;
        using (var reader = ExecuteReaderCommand(command))
        {
          product = ParseProduct(reader);
        }

        return product;
      }
    }

    public int RemoveProduct(int id)
    {
      using (var command = Connection.CreateCommand())
      {
        command.CommandText = $"DELETE FROM tProduct WHERE ID = @ID";
        command.Parameters.AddWithValue("ID", id);
        return ExecuteNonQueryCommand(command);
      }
    }

    public IEnumerable<IProduct> GetProducts()
    {
      using (var command = Connection.CreateCommand())
      {
        command.CommandText = "SELECT * FROM tProduct";

        var list = new List<Product>();
        using (var reader = ExecuteReaderCommand(command))
        {
          while (reader.Read())
          {
            list.Add(ParseProduct(reader));
          }
        }

        return list;
      }
    }

    public IPicture GetPicture(string name)
    {
      var path = $"Content/{DatabaseSettings.VendorName}/{name}";

      if (File.Exists(path))
      {
        return new Picture() {Path = path, Extension = Path.GetExtension(name).Replace(".",string.Empty)};
      }

      return null;
    }

    private Product ParseProduct(IDataRecord reader)
    {
      var id = reader.GetInt32(reader.GetOrdinal("ID"));
      var name = reader["Name"].ToString();
      var price = reader.GetDecimal(reader.GetOrdinal("Price"));
      var quantity = reader.GetInt32(reader.GetOrdinal("Quantity"));
      var picture = reader["Picture"].ToString();
      var unitType = ((UnitType) reader.GetByte(reader.GetOrdinal("UnitID"))).ToString();
      var server = "http://localhost:5000";
      return new Product()
      {
        Id = id,
        Name = name,
        Price = price,
        Quantity = quantity,
        Picture = Path.Combine(server, $"api/products/content?vendorname={DatabaseSettings.VendorName}&name={picture}"),
        UnitType = unitType
      };
    }
  }
}