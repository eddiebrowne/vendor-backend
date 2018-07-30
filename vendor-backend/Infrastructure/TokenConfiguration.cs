using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Infrastructure
{
  public class TokenConfiguration 
  {
    public string Issuer { get; set; }
    public string Key { get; set; }
  }
}