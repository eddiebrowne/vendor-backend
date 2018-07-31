using System;
using System.Net.Http;
using System.Threading.Tasks;
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
    protected string SecurityToken;
    protected string Path;
    protected Login Login = new Login
    {
      Email = $"fake{Stamp}@email.edu",
      Password = "somepassword"
    };

    protected static string Stamp => (DateTime.Now.ToFileTimeUtc().ToString());
    
    private StringContent PrepareContent(object data)
    {
      return JsonContent.Create(JsonConvert.SerializeObject(data));
    }

    protected async Task LoginRequest()
    {
      SecurityToken = await ParseResponse(await TestClient.PostAsync("api/accounts/login", PrepareContent(Login)));
    }

    private async Task<HttpRequestMessage> Request(HttpMethod method, string path = null, object data = null)
    {
      var request = new HttpRequestMessage(method, path ?? Path);
      
      if (string.IsNullOrEmpty(SecurityToken))
      {
        await LoginRequest();
      }
      
      request.Headers.Add("Authorization", "bearer " + SecurityToken);

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
    
    protected HttpRequestMessage GetRequest()
    {
      return Request(HttpMethod.Get).Result;
    }

    protected async Task<HttpRequestMessage> PostRequest(object data, string path = null)
    {
      return await Request(HttpMethod.Post, path, data);
    }
    
    protected HttpRequestMessage DeleteRequest()
    {
      return Request(HttpMethod.Delete).Result;
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
    
    
  }
}