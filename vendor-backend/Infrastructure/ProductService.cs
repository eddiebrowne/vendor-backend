using WebApplication;

namespace Infrastructure
{
  public class ProductService
  {
    private readonly DatabaseAccessLayer _dal = new DatabaseAccessLayer();

    public int Create(Product product)
    {
      var id = _dal.AddProduct(product);
      
      return id;
    }
  }
}