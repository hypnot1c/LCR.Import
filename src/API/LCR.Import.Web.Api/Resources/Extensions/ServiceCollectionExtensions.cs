using Helpers.AspNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

namespace LCR.Import.Web.Api.Resources
{
  internal static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddAndConfigureMvc(this IServiceCollection services)
    {
      services.AddMvc(opt =>
      {
        var cacheProfile = new CacheProfile()
        {
          NoStore = true
        };
        opt.CacheProfiles.Add("Default", cacheProfile);
        opt.Filters.Add<GlobalExceptionFilter>();

      })
      .AddJsonOptions(o =>
      {
        o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
      })
      .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
      ;

      return services;
    }
  }
}
