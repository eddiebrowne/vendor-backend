using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using vendor_backend;
using WebApplication;
using Xunit;
using Domain;

namespace EndpointTests
{
  public class ProductTests : TestBase
  {
    private new const string Path = @"api/products";

    [Fact]
    public async Task Should_Add_Product()
    {
      // Arrange
      var expected = $"{Path}/1";
      var product = new Product
      {
        Name = "test",
        Price = new decimal(10.50),
        Quantity = 3,
        UnitType = UnitType.Each
      };
      
      // Act
      var actual = await ParseResponse(await TestClient.SendAsync(await PostRequest(product)));
      
      // Assert
      Assert.Equal(expected, actual);
    }

    [Fact]
    public async void Should_Remove_Product()
    {
      // Arrange
      const string expected = "Deleted";
      await Should_Add_Product();
      
      // Act
      var actual = await ParseResponse(await TestClient.SendAsync(DeleteRequest()));
      
      // Assert
      Assert.Equal(expected, actual);
    }

    

    [Fact]
    public async void Should_Get_Product_List()
    {
      // Arrange
      await Should_Add_Product();
      await Should_Add_Product();
      
      // Act
      var result = await ParseResponse(await TestClient.SendAsync(GetRequest()));
      var actual = JsonConvert.DeserializeObject<List<Product>>(result);
      
      // Assert
      Assert.True(actual.Count >= 2);
    }

    
  }
}