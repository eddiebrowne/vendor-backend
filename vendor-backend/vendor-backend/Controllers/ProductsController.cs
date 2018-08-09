using System.IO;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Domain;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;

namespace WebApplication.Controllers
{
  [Route("api/[controller]")]
  public class ProductsController : Controller
  {
    private static string Path => "api/products";
    private readonly IProductService _service;
    private readonly IHostingEnvironment _environment;

    public ProductsController(IHostingEnvironment environment, IProductService service)
    {
      _service = service;
      _environment = environment;
    }

    [HttpGet("admin")]
    [Authorize]
    public JsonResult Get()
    {
      return new JsonResult(_service.GetProducts());
    }

    [HttpGet("admin/{id}")]
    [Authorize]
    public JsonResult Get(int id)
    {
      return new JsonResult(_service.GetProduct(id));
    }

    [HttpGet("content")]
    public IActionResult GetPicture([FromQuery] string name)
    {
      var picture = _service.GetPicture(name);
      if (picture == null)
      {
        return new NotFoundResult();
      }

      return PhysicalFile($"{_environment.ContentRootPath}/{picture.Path}", $"image/{picture.Extension}");
    }
    
    [HttpGet]
    public JsonResult GetVendorProducts()
    {
      return new JsonResult(_service.GetProducts());
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