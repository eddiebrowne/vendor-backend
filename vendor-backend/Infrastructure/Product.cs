using Domain;
using Newtonsoft.Json;

namespace Infrastructure
{
  public class Product : IProduct
  {
    [JsonProperty("Name")]
    public string Name { get; set; }

    [JsonProperty("Quantity")]
    public int Quantity { get; set; }

    [JsonProperty("Price")]
    public decimal Price { get; set; }

    [JsonProperty("UnitType")]
    public UnitType UnitType { get; set; }
  }
}