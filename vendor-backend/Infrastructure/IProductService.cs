using System.Collections.Generic;
using WebApplication;

namespace Infrastructure
{
  public interface IProductService
  {
    int Create(Product product);
    Product GetProduct(int id);
    int DeleteProduct(int id);
    IEnumerable<Product> GetProducts();
  }
}