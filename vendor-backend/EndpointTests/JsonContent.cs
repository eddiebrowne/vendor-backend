using System.Net.Http;
using System.Text;

namespace EndpointTests
{
  public static class JsonContent
  {
    public static StringContent Create(string json)
    {
      return new StringContent(json, Encoding.UTF8, "application/json");
    }
  }
}