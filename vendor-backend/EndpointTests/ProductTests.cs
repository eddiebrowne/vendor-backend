using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
using Newtonsoft.Json;
using Xunit;
using Domain;

namespace EndpointTests
{
  public class ProductTests : TestBase
  {
    protected override string Path => @"api/products";
    private int _quantity = 0;

    [Fact]
    public async Task Should_Add_Product()
    {
      // Arrange
      await AddAccount();
      var expected = $"{Path}/{++_quantity}";
      var product = new Product
      {
        Name = "test",
        Price = new decimal(10.50),
        Quantity = 3,
        UnitType = UnitType.Each.ToString()
      };
      
      // Act
      var request = await PostRequest(product, Path);
      var response = await TestClient.SendAsync(request);
      var actual = await ParseResponse(response);
      
      // Assert
      Assert.Equal(expected, actual);
    }
    
    [Fact]
    public async Task Should_Not_Add_Product_When_Not_Authorized()
    {
      // Arrange
      const HttpStatusCode expected = HttpStatusCode.Unauthorized;
      var product = new Product
      {
        Name = "never",
        Price = new decimal(0.50),
        Quantity = 1,
        UnitType = UnitType.Box.ToString()
      };
      
      // Act
      var request = await PostRequest(product, Path, secure: false);
      var response = await TestClient.SendAsync(request);
      var actual = await ParseResponse(response);
      
      // Assert
      Assert.Equal(expected, response.StatusCode);
    }

    [Fact]
    public async void Should_Remove_Product()
    {
      // Arrange
      await Should_Add_Product();
      
      const string expected = "Deleted";
      
      // Act
      var actual = await ParseResponse(await TestClient.SendAsync(await DeleteRequest(1)));
      
      // Assert
      Assert.Equal(expected, actual);
    }

    [Fact]
    public async void Should_Get_Product()
    {
      // Arrange
      await Should_Add_Product();
      
      // Act
      var result = await ParseResponse(await TestClient.SendAsync(await GetRequest($"{Path}/admin/1")));
      var actual = JsonConvert.DeserializeObject<Product>(result);
      
      // Assert
      Assert.NotNull(actual);
    }
    
    [Fact]
    public async void Should_Get_Product_List_As_Admin()
    {
      // Arrange
      await Should_Add_Product();
      await Should_Add_Product();
      
      // Act
      var result = await ParseResponse(await TestClient.SendAsync(await GetRequest($"{Path}/admin")));
      var actual = JsonConvert.DeserializeObject<List<Product>>(result);
      
      // Assert
      Assert.True(actual.Count >= 2);
    }
    
    [Fact]
    public async void Should_Get_Product_List_As_Customer()
    {
      // Arrange
      await Should_Add_Product();
      await Should_Add_Product();
      
      // Act
      var request = await GetRequest($"{Path}?vendor={VendorName}");
      var result = await ParseResponse(await TestClient.SendAsync(request));
      var actual = JsonConvert.DeserializeObject<List<Product>>(result);
      
      // Assert
      Assert.True(actual.Count >= 2);
    }
  }
}