using System;
using System.Data.SQLite;
using System.IO;
using System.Text;
using Domain;
using Domain.Services;
using Infrastructure;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Binder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace vendor_backend
{
  public class Startup
  {
    private TokenSettings TokenSettings { get; set; }
    private DatabaseSettings DatabaseSettings { get; set; }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddAuthentication().AddJwtBearer(options =>
      {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
          ValidIssuer = TokenSettings.Issuer,
          ValidAudience = TokenSettings.Issuer,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenSettings.Key))
        };
      });
      services.AddMvc();
      services.AddSingleton(_ => TokenSettings);
      services.AddSingleton(_ => DatabaseSettings);
      services.AddTransient<IProductRepository, ProductRepository>();
      services.AddTransient<IAccountRepository, AccountRepository>();
      services.AddTransient<IProductService, ProductService>();
      services.AddTransient<IAccountService, AccountService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);

      var config = builder.Build();
      TokenSettings = config.GetSection("token").Get<TokenSettings>();
      TokenSettings.SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(TokenSettings.Key)), SecurityAlgorithms.HmacSha256Signature);
      
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        builder.AddUserSecrets<Startup>();
      }

      app.UseAuthentication();
      
      app.Use((context, next) =>
      {
        if (context.Request.GetUri().AbsoluteUri.Contains("localhost"))
        {
          if (DatabaseSettings == null)
          {
            DatabaseSettings = new DatabaseSettings();
            var thing = config.GetSection("db:test");
            thing.Bind(DatabaseSettings);
          }

          var testDb = $"{DatabaseSettings.ConnectionString.Split("=")[1]}";
          if (!File.Exists(testDb))
          {
            SQLiteConnection.CreateFile(testDb);
            var script = File.ReadAllText(DatabaseSettings.Scripts.Main);
            RepositoryBase.RunScript(script, DatabaseSettings.ConnectionString);
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

          var service = (IAccountService) app.ApplicationServices.GetService(typeof(IAccountService));
          var vendor = service.GetVendorFromToken(token);

          DatabaseSettings.ConnectionString = string.Format(config.GetSection("db-vendor").ToString(), vendor.Name);
        }

        return next();
      });


      app.UseMvc(); //.Run(async (context) => { await context.Response.WriteAsync("Hello World!"); });
    }
  }
}