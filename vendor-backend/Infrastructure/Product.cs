using Domain;
using Newtonsoft.Json;

namespace Infrastructure
{
  public class Product : IProduct
  {
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("quantity")]
    public int Quantity { get; set; }

    [JsonProperty("price")]
    public decimal Price { get; set; }

    [JsonProperty("unit")]
    public string UnitType { get; set; }

    [JsonProperty("count")]
    public int Count { get; set; }
    
    [JsonProperty("order")]
    public int Order { get; set; }

    [JsonProperty("picture")]
    public string Picture { get; set; }
    
    [JsonProperty("id")]
    public int Id { get; set; }
  }
}