using Domain.Services;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
  [Route("api/[controller]")]
  public class OrdersController : Controller
  {
    private static string Path => "api/orders";
    private readonly IOrderService _service;

    public OrdersController(IOrderService service)
    {
      _service = service;
    }
    
    [HttpPost]
    public IActionResult ProcessOrder([FromBody] Order order)
    {
      if(_service.ProcessOrder(order))
        return new OkResult();
      
      return new UnprocessableEntityResult();
    }
  }
}