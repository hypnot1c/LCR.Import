using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace LCR.Import.UI.Web
{
  public class Program
  {
    public static void Main(string[] args)
    {
      BuildWebHost(args).Run();
    }

    public static IWebHost BuildWebHost(string[] args)
    {
      return WebHost.CreateDefaultBuilder(args)
          .ConfigureAppConfiguration((hostingContext, config) =>
          {
            var env = hostingContext.HostingEnvironment;
            config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

            if (env.IsDevelopment())
            {
              config.AddUserSecrets<Startup>();
            }

            config.AddEnvironmentVariables();
          })
          .ConfigureLogging((hostingContext, logging) =>
          {
            var env = hostingContext.HostingEnvironment;
            logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));

            if (env.IsDevelopment())
            {
              logging.AddDebug();
            }

            logging.AddNLog($"nlog.{env.EnvironmentName}.config");
          })
          .UseStartup<Startup>()
          .Build();
    }
  }
}
