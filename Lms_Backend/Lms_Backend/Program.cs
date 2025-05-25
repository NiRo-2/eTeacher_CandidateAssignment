using NLog;
using NLog.Web;

namespace Lms_Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = LogManager.Setup().LoadConfigurationFromFile("NLog.config").GetCurrentClassLogger();

            try
            {
                var builder = WebApplication.CreateBuilder(args);
                logger.Info("Lms_Backend init");

                // Configure NLog for Dependency injection abd remove the default ASP.NET Core logging providers
                builder.Logging.ClearProviders();
                builder.Host.UseNLog();

                //startup and configure services
                var startup = new Startup(builder.Configuration);
                startup.ConfigureServices(builder.Services);

                var app = builder.Build();
                //add swagger support only for development mode
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                //configures
                startup.Configure(app, app.Environment);
                app.Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Application startup failed");
                Environment.Exit(1);
            }
            finally
            {
                logger.Info("Application is shutting down");
                LogManager.Shutdown();
            }
        }
    }
}