using System.Collections.Generic;

namespace Domain
{
  public interface IProductService
  {
    int Create(IProduct product);
    IProduct GetProduct(int id);
    int DeleteProduct(int id);
    IEnumerable<IProduct> GetProducts();
  }
}