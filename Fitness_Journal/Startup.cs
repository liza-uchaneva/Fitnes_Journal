using Fitness_Journal.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Fitness_Journal
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; protected set; }
        public Startup(IWebHostEnvironment env)
        {
            this.Environment = env;
            var builder = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
            services.AddAuthorizationBuilder();

            var connectionString = Configuration.GetConnectionString("FitnessJournalConnection")
            ?? throw new InvalidOperationException("Connection string 'FitnessJournalConnection' not found.");
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            services.AddIdentityCore<User>()
                   .AddEntityFrameworkStores<ApplicationDbContext>()
                   .AddApiEndpoints();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapIdentityApi<User>();
                endpoints.MapGet("/", (ClaimsPrincipal user) => $"Hello {user.Identity!.Name}")
                         .RequireAuthorization();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
