using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Infrastructure;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;

namespace Domain
{
  public class AccountService : IAccountService
  {
    private readonly IAccountRepository _repository;
    private readonly TokenSettings _tokenSettings;

    public AccountService(IAccountRepository repository, TokenSettings tokenSettings)
    {
      _repository = repository;
      _tokenSettings = tokenSettings;
    }
    
    public int Create(IAccount account)
    {
      var result = _repository.Create(account);
      if (result > 0)
      {
        _repository.CreateDatabase(account.Name);
      }

      return result;
    }

    public IAccount GetVendorAccount(string email, string password)
    {
      return _repository.GetAccount(email, password);
    }

    public IAccount GetVendorFromToken(string token)
    {
      return _repository.GetAccountFromToken(token);
    }

    public string CreateToken(IAccount account)
    {
      var jwtPayload = new JwtPayload()
      {
        { "email", account.Email }
      };
      var jwt = new JwtSecurityToken(new JwtHeader(_tokenSettings.SigningCredentials), jwtPayload);
      return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
  }
}