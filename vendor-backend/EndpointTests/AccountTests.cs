using System.IO;
using System.Threading.Tasks;
using Domain;
using Infrastructure;
using Newtonsoft.Json;
using Xunit;

namespace EndpointTests
{
  public class AccountTests : TestBase
  {
    private new const string Path = @"api/accounts";

    [Fact]
    public async Task Should_Add_Account()
    {
      // Arrange
      var account = new Account
      {
        Name = $"test-{Stamp}",
        Email = Login.Email,
        Password = Login.Password
      };
      
      var content = JsonContent.Create(JsonConvert.SerializeObject(account));
      
      // Act
      var actual = await (await TestClient.PostAsync(Path, content)).Content.ReadAsStringAsync();
      
      // Assert
      Assert.True(int.Parse(actual.Split("/")[2]) > 0);
    }
    
    [Fact]
    public async Task Should_Login()
    {
      // Arrange
      await Should_Add_Account();
      
      // Act
      await LoginRequest();
      
      // Assert
      Assert.NotEmpty(SecurityToken);
    }
  }
}