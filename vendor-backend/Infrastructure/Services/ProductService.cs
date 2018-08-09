using System.Collections.Generic;
using Domain;
using Domain.Services;

namespace Infrastructure.Services
{
  public class ProductService : IProductService
  {
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
      _productRepository = productRepository;
    }
    
    public int Create(IProduct product)
    {
      return _productRepository.AddProduct(product);
    }

    public IProduct GetProduct(int id)
    {
      return _productRepository.GetProduct(id);
    }

    public int DeleteProduct(int id)
    {
      return _productRepository.RemoveProduct(id);
    }

    public IEnumerable<IProduct> GetProducts()
    {
      return _productRepository.GetProducts();
    }

    public IPicture GetPicture(string name)
    {
      return _productRepository.GetPicture(name);
    }
  }
}