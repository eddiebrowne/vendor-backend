using System.Configuration;
using MySql.Data.MySqlClient;
using WebApplication;

namespace Infrastructure
{
  internal class DatabaseAccessLayer
  {
    private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["vendordb"].ToString();
    private readonly MySqlConnection _connection = new MySqlConnection(ConnectionString);
    
    public int AddProduct(Product product)
    {
      var command = _connection.CreateCommand();
      command.CommandText = "INSERT INTO tProducts(Name, Quantity, Price, UnitID) VALUES({0}, {1}, {2}, {3})";
      command.Parameters.Add(new MySqlParameter("Name", MySqlDbType.VarChar) {Value = product.Name});
      command.Parameters.Add(new MySqlParameter("Quantity", MySqlDbType.Int32) {Value = product.Quantity});
      command.Parameters.Add(new MySqlParameter("Price", MySqlDbType.Decimal) {Value = product.Price});
      command.Parameters.Add(new MySqlParameter("UnitID", MySqlDbType.Byte) {Value = product.UnitType});

      return (int) command.ExecuteScalar();
    }
  }
}