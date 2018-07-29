using System;
using System.Data.SQLite;
using System.IO;
using Domain;
using Infrastructure;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IDatabase = Domain.IDatabase;

namespace vendor_backend
{
  public class Startup
  {
    public Config Configuration { get; private set; }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc();
      services.AddSingleton(_ => Configuration);
      services.AddTransient<IDatabase, Database>();
      services.AddTransient<IProductService, ProductService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true);

      var config = builder.Build();


      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        builder.AddUserSecrets<Startup>();
      }

      app.Use((context, next) =>
      {
        if (context.Request.GetUri().AbsoluteUri.Contains("localhost"))
        {
          if (Configuration == null)
          {
            var connectionString = config["db-main.test:ConnectionString"];
            Configuration = new Config() {ConnectionString = connectionString};
          }

          var testDb = $"{Configuration.ConnectionString.Split("=")[1]}.sql";
          if (!File.Exists(testDb))
          {
            SQLiteConnection.CreateFile(testDb);
            var script = File.ReadAllText("../../../../vendor-backend/database-vendor.sql");
            Database.RunScript(script, Configuration.ConnectionString);
          }
        }

        return next();
      });

      app.Use((context, next) =>
      {
        var token = context.Request.Headers["token"];

        if (string.IsNullOrEmpty(token))
        {
          string authorization = context.Request.Headers["Authorization"];

          if (string.IsNullOrEmpty(authorization))
          {
            return next();
          }

          if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
          {
            token = authorization.Substring("Bearer ".Length).Trim();
          }

          var vendor = AccountService.GetVendorFromToken(token);

          Configuration.ConnectionString = string.Format(config.GetSection("db-vendor").ToString(), vendor.Name);
        }

        return next();
      });


      app.UseMvc(); //.Run(async (context) => { await context.Response.WriteAsync("Hello World!"); });
    }
  }
}