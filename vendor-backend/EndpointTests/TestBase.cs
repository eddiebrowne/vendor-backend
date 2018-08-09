using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Domain;
using Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using vendor_backend;
using Xunit;

namespace EndpointTests
{
  public abstract class TestBase
  {
    protected TokenResponse SecurityTokenResponse;
    protected abstract string Path { get; }

    private string _stamp;
    private string Stamp => _stamp ?? (_stamp = (DateTime.Now.ToFileTimeUtc().ToString()));

    private IAccount Account { get; set; }

    private Login _login;
    protected string VendorName => $"test-{Stamp}";

    private Login Login
    {
      get
      {
        if (_login != null) return _login;
        _login = new Login
        {
          Email = $"fake{Stamp}@email.edu",
          Password = "somepassword"
        };

        return _login;
      }
    }

    private StringContent PrepareContent(object data)
    {
      return JsonContent.Create(JsonConvert.SerializeObject(data));
    }

    protected async Task LoginRequest()
    {
      SecurityTokenResponse =
        JsonConvert.DeserializeObject<TokenResponse>(
          await ParseResponse(await TestClient.PostAsync("api/accounts/login", PrepareContent(Login))));
    }

    private async Task<HttpRequestMessage> Request(
      HttpMethod method, 
      string path = null, 
      object data = null,
      bool secure = true)
    {
      var request = new HttpRequestMessage(method, $"{TestClient.BaseAddress}{path}");

      if (secure)
      {
        if(SecurityTokenResponse == null)
        {
          await LoginRequest();  
        }
        request.Headers.Add("Authorization", "bearer " + SecurityTokenResponse.Token);
      }
      
      if (data != null)
      {
        request.Content = PrepareContent(data);
      }

      return request;
    }

    protected async Task<string> ParseResponse(HttpResponseMessage response)
    {
      return await response.Content.ReadAsStringAsync();
    }

    protected async Task<HttpRequestMessage> GetRequest(string path = null, object data = null)
    {
      return await Request(HttpMethod.Get, path, data);
    }

    protected async Task<HttpRequestMessage> PostRequest(object data, string path = null, bool secure = true)
    {
      return await Request(HttpMethod.Post, path, data, secure);
    }

    protected async Task<HttpRequestMessage> DeleteRequest(int id)
    {
      var path = $"{Path}/{id}";
      return await Request(HttpMethod.Delete, path: path);
    }

    protected static HttpClient TestClient
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

    public async Task AddAccount()
    {
      await Should_Add_Account();
    }
  
    [Fact]
    private async Task Should_Add_Account()
    {
      // Arrange
      if (Account == null)
      {
        Account = new Account
        {
          Name = VendorName,
          Email = Login.Email,
          Password = Login.Password
        };
      
        var content = JsonContent.Create(JsonConvert.SerializeObject(Account));
      
        // Act
        var actual = await (await TestClient.PostAsync("api/accounts", content)).Content.ReadAsStringAsync();
      
        // Assert
        Assert.True(int.Parse(actual.Split("/")[2]) > 0);
      }
    }
  }

  public class TokenResponse
  {
    public string Token { get; set; }
  }
}