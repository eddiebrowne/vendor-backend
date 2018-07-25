using System.Collections.Generic;
using WebApplication;

namespace Infrastructure
{
  public class ProductService : IProductService
  {
    private readonly IDatabase _database;

    public ProductService(IDatabase database)
    {
      _database = database;
    }
    
    public int Create(Product product)
    {
      return _database.AddProduct(product);
    }

    public Product GetProduct(int id)
    {
      return _database.GetProduct(id);
    }

    public int DeleteProduct(int id)
    {
      return _database.RemoveProduct(id);
    }

    public IEnumerable<Product> GetProducts()
    {
      return _database.GetProducts();
    }
  }
}