using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using WebApplication;

namespace Infrastructure
{
  public interface IDatabase
  {
    int AddProduct(Product product);
    Product GetProduct(int id);
    int RemoveProduct(int id);
    IEnumerable<Product> GetProducts();
  }
}