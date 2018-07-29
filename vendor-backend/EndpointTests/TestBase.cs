using System;
using System.Data.SQLite;
using System.IO;
using System.Net.Http;
using Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using vendor_backend;

namespace EndpointTests
{
  public class TestBase
  {
    protected TestBase()
    {
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