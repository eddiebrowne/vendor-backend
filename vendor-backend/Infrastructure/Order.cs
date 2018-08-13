using System;
using System.Collections.Generic;
using Domain.Services;
using Newtonsoft.Json;

namespace Infrastructure
{
  public class Order : IOrder
  {
    [JsonProperty("vendorID")]
    public string VendorId { get; set; }
    
    [JsonProperty("customer")]
    public string Customer { get; set; }

    [JsonProperty("Market")]
    public string Market { get; set; }
    
    [JsonProperty("time")]
    public DateTime PickupTime { get; set; }

    [JsonProperty("items")]
    public IList<IItem> Items { get; set; }
  }
}