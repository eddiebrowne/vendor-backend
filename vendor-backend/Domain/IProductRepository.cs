using System.Collections.Generic;

namespace Domain
{
  public interface IProductRepository
  {
    int AddProduct(IProduct product);
    IProduct GetProduct(int id);
    int RemoveProduct(int id);
    IEnumerable<IProduct> GetProducts();
  }
}