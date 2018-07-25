using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using vendor_backend;
using WebApplication;
using Xunit;

namespace EndpointTests
{
  public class ProductTests : TestBase
  {
    private const string Path = @"api/products";

    private static HttpClient TestClient
    {
      get
      {
        var builder = new WebHostBuilder().UseStartup<Startup>();
        var server = new TestServer(builder)
        {
          BaseAddress = new Uri("http://localhost:5000")
        };
        return server.CreateClient();
      }
    }

    [Fact]
    public async void Should_Add_Product()
    {
      // Arrange
      const string expected = "OK";
      var product = new Product();

      var content = JsonContent.Create(JsonConvert.SerializeObject(product));
      
      // Act
      var actual = await (await TestClient.PostAsync(Path, content)).Content.ReadAsStringAsync();
      
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