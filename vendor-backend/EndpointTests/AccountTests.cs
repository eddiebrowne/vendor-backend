using System.Threading.Tasks;
using Xunit;

namespace EndpointTests
{
  public class AccountTests : TestBase
  {
    protected override string Path => @"api/accounts";

    [Fact]
    public async Task Should_Login()
    {
      // Arrange
      await AddAccount();
      
      // Act
      await LoginRequest();
      
      // Assert
      Assert.NotNull(SecurityTokenResponse);
      Assert.True(SecurityTokenResponse.Token.Length > 0);
    }
  }
}