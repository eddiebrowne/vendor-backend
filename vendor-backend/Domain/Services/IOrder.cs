using System;
using System.Collections.Generic;

namespace Domain.Services
{
  public interface IOrder
  {
    string VendorId { get; set; }
    string Customer { get; set; }
    string Market { get; set; }
    DateTime PickupTime { get; set; }
    IList<IItem> Items { get; set; }
  }
}