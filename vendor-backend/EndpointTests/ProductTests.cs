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

namespace EndpointTests
{
  public class ProductTests : TestBase
  {
    private const string Path = @"api/products";

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
      
      var content = JsonContent.Create(JsonConvert.SerializeObject(product));
      
      // Act
      var actual = await (await TestClient.PostAsync(Path, content)).Content.ReadAsStringAsync();
      
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
      var actual = await (await TestClient.DeleteAsync($@"{Path}\1")).Content.ReadAsStringAsync();
      
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
      var result = await (await TestClient.GetAsync($@"{Path}")).Content.ReadAsStringAsync();
      var actual = JsonConvert.DeserializeObject<List<Product>>(result);
      
      // Assert
      Assert.True(actual.Count >= 2);
    }
  }
}