using Lms_Backend.Interfaces;
using Lms_Backend.Models;
using Lms_Backend.Services;

namespace Lms_Backend
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // Register services
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // Register application services
            services.AddSingleton<IDataContext, DataContext>();
            services.AddSingleton<IEnrollmentService, EnrollmentService>();
            services.AddSingleton<IStudentService, StudentService>();
            services.AddSingleton<ICourseService, CourseService>();

            //Add CORS support
            var allowedOrigins = Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? new string[] { };
            services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins(allowedOrigins)
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
        }

        // Configure middleware
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("AllowFrontend");//cors - allow frontend access
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //Adding fake data to the in-memory database
            var context = app.ApplicationServices.GetRequiredService<IDataContext>();
            DbSeeder.Seed(context);
        }
    }
}