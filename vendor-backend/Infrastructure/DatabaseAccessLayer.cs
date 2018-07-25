using MySql.Data.MySqlClient;
using WebApplication;

namespace Infrastructure
{
  internal class DatabaseAccessLayer
  {
    private readonly MySqlConnection _connection;

    public DatabaseAccessLayer()
    {
      _connection = new MySqlConnection(builder.ConnectionString);
      //new MySqlConnection(Config.ConnectionString);
    }
    
    public int AddProduct(Product product)
    {
      using (var connection = new MySqlConnection(Config.ConnectionString))
      {
        connection.Open();
        var command = _connection.CreateCommand();
        command.CommandText = "INSERT INTO tProducts(Name, Quantity, Price, UnitID) VALUES(@Name, @Quantity, @Price, @UnitID)";
        command.Parameters.Add(new MySqlParameter("Name", MySqlDbType.VarChar) {Value = product.Name});
        command.Parameters.Add(new MySqlParameter("Quantity", MySqlDbType.Int32) {Value = product.Quantity});
        command.Parameters.Add(new MySqlParameter("Price", MySqlDbType.Decimal) {Value = product.Price});
        command.Parameters.Add(new MySqlParameter("UnitID", MySqlDbType.Byte) {Value = product.UnitType});

        var result = command.ExecuteNonQuery();
        return result;
      }
    }
  }
}