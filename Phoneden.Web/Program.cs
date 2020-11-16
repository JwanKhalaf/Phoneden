namespace Phoneden.Web
{
  using System;
  using System.Net.Sockets;
  using DataAccess.Context;
  using DataAccess.Initialisations;
  using Entities;
  using Microsoft.AspNetCore.Hosting;
  using Microsoft.AspNetCore.Identity;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.Extensions.Logging;
  using System.Threading.Tasks;
  using Microsoft.Extensions.Hosting;
  using Npgsql;
  using Polly;
  using Polly.Retry;

  public class Program
  {
    public static async Task Main(string[] args)
    {
      IHost hostBuilder = CreateHostBuilder(args).Build();

      using (IServiceScope scope = hostBuilder.Services.CreateScope())
      {
        IServiceProvider services = scope.ServiceProvider;

        try
        {
          PdContext context = services
            .GetRequiredService<PdContext>();

          UserManager<ApplicationUser> userManager = services
            .GetRequiredService<UserManager<ApplicationUser>>();

          RoleManager<IdentityRole> roleManager = services
            .GetRequiredService<RoleManager<IdentityRole>>();

          ILogger<DbInitializer> dbInitializerLogger = services
            .GetRequiredService<ILogger<DbInitializer>>();

          AsyncRetryPolicy retryPolicy = Policy
            .Handle<SocketException>()
            .Or<PostgresException>()
            .Or<NpgsqlException>()
            .RetryAsync(100);

          await retryPolicy.ExecuteAsync(() =>
            DbInitializer.Initialize(context, userManager, roleManager, dbInitializerLogger));
        }
        catch (Exception ex)
        {
          ILogger<Program> logger = services
            .GetRequiredService<ILogger<Program>>();

          logger
            .LogError(ex, "An error occurred while migrating the database.");
        }
      }

      await hostBuilder.RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
      Host
        .CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
          webBuilder.UseStartup<Startup>();
        });
  }
}
