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
    private readonly string _databaseFileName = $"DB-{DateTime.Now.ToLongDateString()}.sqlite";

    protected TestBase()
    {
      InitializeDatabase();
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

    private void InitializeDatabase()
    {
      SQLiteConnection.CreateFile(_databaseFileName);
      Config.ConnectionString = $"Data Source={_databaseFileName}";
      var script = File.ReadAllText("../../../../vendor-backend/database.sql");
      Database.RunScript(script);
    }
  }
}