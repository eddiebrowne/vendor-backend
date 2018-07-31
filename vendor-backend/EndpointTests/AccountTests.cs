using System.Threading.Tasks;
using Domain;
using Infrastructure;
using Newtonsoft.Json;
using Xunit;

namespace EndpointTests
{
  public class AccountTests : TestBase
  {
    private const string Path = @"api/accounts";

    [Fact]
    public async Task Should_Add_Account()
    {
      // Arrange
      var expected = $"{Path}/1";
      var account = new Account
      {
        Name = "test",
        Email = "fake@email.edu",
        Password = "somepassword"
      };
      
      var content = JsonContent.Create(JsonConvert.SerializeObject(account));
      
      // Act
      var actual = await (await TestClient.PostAsync(Path, content)).Content.ReadAsStringAsync();
      
      // Assert
      Assert.Equal(expected, actual);
    }
  }
}