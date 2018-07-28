using Microsoft.AspNetCore.Mvc;
using Domain;
using Infrastructure;

namespace WebApplication.Controllers
{
  [Route("api/[controller]")]
  public class AccountsController : Controller
  {
    private readonly IAccountService _service;
    
    public AccountsController(IAccountService service)
    {
      _service = service;
    }
    
    [HttpPost]
    public string Create([FromBody]Account account)
    {
      var id = _service.Create(account);
      return $@"api/accounts/{id}";
    }
  }
}