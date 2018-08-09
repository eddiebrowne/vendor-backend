using System;
using System.Collections.Generic;

namespace Domain.Services
{
  public interface IOrder
  {
    string VendorId { get; set; }
    string Customer { get; set; }
    string Location { get; set; }
    DateTime PickupTime { get; set; }
    IList<IItem> Items { get; set; }
  }
}