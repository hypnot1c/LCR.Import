using Helpers.AspNet;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LCR.Import.UI.Web
{
  public class Startup
  {
    public Startup(IHostingEnvironment env, IConfiguration config)
    {
      this.HostingEnvironment = env;
      this.Configuration = config;
    }

    public IConfiguration Configuration { get; }
    protected IHostingEnvironment HostingEnvironment { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddHttpContextAccessor();

      services.AddMvc(opt =>
      {
        opt.Filters.Add(typeof(GlobalExceptionFilter));
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseStaticFiles();

      app.UseMvc(routes =>
      {
        routes.MapRoute(
                  name: "areaRoute",
                  template: "{area:exists}/{controller=Home}/{action=Index}"
              );

        routes.MapRoute(
                  name: "default",
                  template: "{controller=Home}/{action=Index}/{id?}"
              );

        routes.MapSpaFallbackRoute(
                  name: "spa-default",
                  defaults: new { controller = "Home", action = "Index" }
              );
      });
    }
  }
}
