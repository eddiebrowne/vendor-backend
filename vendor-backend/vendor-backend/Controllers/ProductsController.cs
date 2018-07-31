using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Domain;
using Microsoft.AspNetCore.Authorization;

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
    [Authorize]
    public IEnumerable<IProduct> Get()
    {
      return _service.GetProducts();
    }

    [HttpGet("{id}")]
    [Authorize]
    public string Get(int id)
    {
      return JsonConvert.SerializeObject(_service.GetProduct(id));
    }

    [HttpPost]
    [Authorize]
    public string Post([FromBody] Product product)
    {
      return $"{Path}/{_service.Create(product)}";
    }

    [HttpDelete("{id}")]
    [Authorize]
    public string Delete(int id)
    {
      return _service.DeleteProduct(id) > 0 ? "Deleted" : "Error";
    }
  }
}