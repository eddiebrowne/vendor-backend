using System.Collections.Generic;
using Domain.Services;

namespace Infrastructure
{
  public class Vendor : IVendor
  {
    public List<string> Locations { get; set; }
  }
}