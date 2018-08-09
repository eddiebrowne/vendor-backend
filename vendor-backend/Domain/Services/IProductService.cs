using System.Collections.Generic;

namespace Domain.Services
{
  public interface IProductService
  {
    int Create(IProduct product);
    IProduct GetProduct(int id);
    int DeleteProduct(int id);
    IEnumerable<IProduct> GetProducts();
    IPicture GetPicture(string name);
  }
}