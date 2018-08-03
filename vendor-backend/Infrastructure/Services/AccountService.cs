using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Domain.Services;
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
    
    public long Create(IAccount account)
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

    public string GenerateToken(IAccount account)
    {
      var guid = Guid.NewGuid().ToString();
      var token = CreateToken(guid);
      _repository.StoreToken(account, guid);
      
      return token;
    }

    private string CreateToken(string guid)
    {
      var handler = new JwtSecurityTokenHandler();
      var now = DateTime.Now;
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new[]
        {
          new Claim(JwtRegisteredClaimNames.Jti, guid)
        }),
        Issuer = _tokenSettings.Issuer,
        Audience = _tokenSettings.Audience,
        Expires = now.AddMinutes(_tokenSettings.Expiration),
        SigningCredentials = _tokenSettings.SigningCredentials
      };

      var token = handler.CreateJwtSecurityToken(tokenDescriptor);
      return handler.WriteToken(token);
    }
  }
}