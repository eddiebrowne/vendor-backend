using Domain.Services;
using Newtonsoft.Json;

namespace Infrastructure
{
  public class Item : IItem
  {
    [JsonProperty("id")]
    public int Id { get; set; }
    [JsonProperty("amount")]
    public int Amount { get; set; }
  }
}