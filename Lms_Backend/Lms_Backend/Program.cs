using NLog;
using NLog.Web;

namespace Lms_Backend
{
    public class Program
    {
        //TODO in real life project:
        //IpRateLimiting, Authentication, Authorization, SSL (With auto renewing), Db and etc

        //TODO maybe add aws sdk for S3 demo
        public static void Main(string[] args)
        {
            var logger = LogManager.Setup().LoadConfigurationFromFile("NLog.config").GetCurrentClassLogger();

            try
            {
                logger.Info("Lms_Backend init");

                var builder = WebApplication.CreateBuilder(args);
                // Set URLs from config (appsettings.json)
                string urls = builder.Configuration.GetValue<string>("Urls")??string.Empty;
                builder.WebHost.UseUrls(urls);

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
                    logger.Info("Running on Debug mode");
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }
                else
                {
                    logger.Info("Running on Production mode");
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