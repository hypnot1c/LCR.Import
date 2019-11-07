using AutoMapper;
using LCR.DataService.Abstractions;
using LCR.DataService.Mock;
using LCR.Import.Web.Api.Resources;
using LCR.TPM.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace LCR.Import.Web.Api
{
  /// <summary>
  ///
  /// </summary>
  /// <summary>
  ///
  /// </summary>
  public class Startup
  {
    /// <summary>
    ///
    /// </summary>
    /// <param name="env"></param>
    /// <param name="config"></param>
    public Startup(IHostingEnvironment env, IConfiguration config)
    {
      this.HostingEnvironment = env;
      this.Configuration = config;
    }

    protected IConfiguration Configuration { get; }
    protected IHostingEnvironment HostingEnvironment { get; }

    /// <summary>
    /// This method gets called by the runtime. Use this method to add services to the container.
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddOptions();

      services.AddHttpContextAccessor();

      services.AddAndConfigureMvc();

      services.AddCors();

      services.AddDbContext<TPMContext>(opts =>
          opts.UseOracle(this.Configuration.GetConnectionString(nameof(TPMContext)))
      );

      services.AddAutoMapper(typeof(MappingProfile));

      services.AddScoped<IDataService, MockDataService>();

      services.AddScoped<IProccessFileCommandService<ProccessFileCommand>, ProccessFileCommandService>();
      services.AddHostedService<ProccessFileCommandHostedService>();
      services.AddSingleton<IBackgroundQueue<ICommand>, ProccessFileCommandBackgroundQueue>();
    }

    /// <summary>
    /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    /// </summary>
    /// <param name="app"></param>
    public void Configure(IApplicationBuilder app)
    {
      System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

      if (this.HostingEnvironment.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseStaticFiles();

      app.UseCors(builder =>
      {

        builder.AllowAnyHeader();
        builder.AllowAnyMethod();

        var configSection = this.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
        if (configSection != null)
        {
          builder.WithOrigins(configSection);
        }
        else
        {
          builder.AllowAnyOrigin();
        }

        builder.AllowCredentials();
      });

      app.UseMvc();

      var storageDirectoryPath = this.Configuration.GetValue<string>("StorageDirectory");

      if (!Directory.Exists(storageDirectoryPath))
      {
        Directory.CreateDirectory(storageDirectoryPath);
      }
    }
  }
}
