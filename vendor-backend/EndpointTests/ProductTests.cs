using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebApplication;
using Xunit;

namespace EndpointTests
{
  public class ProductTests
  {
    private readonly HttpClient _testClient = new HttpClient()
    {
      BaseAddress = new Uri(@"http://localhost:5000")
    };

    [Fact]
    public void Should_Add_Product()
    {
      // Arrange
      const string expected = "OK";
      var endpoint = @"api/products";
      var product = new Product();

      var content = new StringContent(JsonConvert.SerializeObject(product));
      
      // Act
      var actual = _testClient.PostAsync(endpoint, content).Result.Content.ToString();
      
      // Assert
      Assert.Equal(expected, actual);
    }

    [Fact]
    public void Should_Remove_Product()
    {
      
    }

    [Fact]
    public void Should_Get_Product_List()
    {
      
    }
  }
}