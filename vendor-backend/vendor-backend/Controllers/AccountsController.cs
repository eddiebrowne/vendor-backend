using Microsoft.AspNetCore.Mvc;
using Domain;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;

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
    [AllowAnonymous]
    public string Create([FromBody]Account account)
    {
      var id = _service.Create(account);
      return $@"api/accounts/{id}";
    }

    [HttpPost]
    [AllowAnonymous]
    public IActionResult Login([FromBody] Login login)
    {
      var account = _service.GetVendorAccount(login.Email, login.Password);
            
      return null;
    }
  }
}