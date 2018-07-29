using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Infrastructure
{
  public class Config 
  {
    public string ConnectionString { get; set; }
  }
}