using System.Collections.Generic;
using Domain;

namespace Infrastructure
{
  public class ProductService : IProductService
  {
    private readonly IDatabase _database;

    public ProductService(IDatabase database)
    {
      _database = database;
    }
    
    public int Create(IProduct product)
    {
      return _database.AddProduct(product);
    }

    public IProduct GetProduct(int id)
    {
      return _database.GetProduct(id);
    }

    public int DeleteProduct(int id)
    {
      return _database.RemoveProduct(id);
    }

    public IEnumerable<IProduct> GetProducts()
    {
      return _database.GetProducts();
    }
  }
}