using LCR.Import.Web.Api.Resources;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
    }

    /// <summary>
    /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    /// </summary>
    /// <param name="app"></param>
    public void Configure(IApplicationBuilder app)
    {
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
    }
  }
}
