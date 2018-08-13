using System.Collections.Generic;
using Domain;
using Domain.Services;

namespace Infrastructure
{
  public class Vendor : IVendor
  {
    public string Name { get; set; }
    public List<IMarket> Markets { get; set; }
  }
}