using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
  [Route("api/[controller]")]
  public class ProductsController : Controller
  {
    private static string Path => "api/products";
    private readonly ProductService _service = new ProductService();

    [HttpGet]
    public IEnumerable<string> Get()
    {
      return new string[] {"value1", "value2"};
    }

    [HttpGet("{id}")]
    public string Get(int id)
    {
      return "value";
    }

    [HttpPost]
    public string Post([FromBody] Product product)
    {
      return $"{Path}/{_service.Create(product)}";
    }


    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
  }
}