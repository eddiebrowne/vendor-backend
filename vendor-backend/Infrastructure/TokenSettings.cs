using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure
{
  public class TokenSettings 
  {
    public string Issuer { get; set; }
    public string Key { get; set; }
    public string Audience { get; set; }
    public int Expiration { get; set; }
    public SigningCredentials SigningCredentials { get; set; }
  }
}