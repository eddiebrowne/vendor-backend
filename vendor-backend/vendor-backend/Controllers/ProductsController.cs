using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Domain;

namespace WebApplication.Controllers
{
  [Route("api/[controller]")]
  public class ProductsController : Controller
  {
    private static string Path => "api/products";
    private readonly IProductService _service;

    public ProductsController(IProductService service)
    {
      _service = service;
    }

    [HttpGet]
    public IEnumerable<IProduct> Get()
    {
      return _service.GetProducts();
    }

    [HttpGet("{id}")]
    public string Get(int id)
    {
      return JsonConvert.SerializeObject(_service.GetProduct(id));
    }

    [HttpPost]
    public string Post([FromBody] Product product)
    {
      return $"{Path}/{_service.Create(product)}";
    }

    [HttpDelete("{id}")]
    public string Delete(int id)
    {
      return _service.DeleteProduct(id) > 0 ? "Deleted" : "Error";
    }
  }
}