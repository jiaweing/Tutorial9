using System.Text.Json;
using System.Text.Json.Serialization;
using Web.Database;
using Web.Services;
using Web.Services.Auth;

namespace Web.Startup
{
    public static class AppServiceRegistration
    {
        public static async Task<IServiceCollection> AddAppServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddJwt(config);
            services.AddControllers()
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    });
            services.AddEndpointsApiExplorer();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddRateLimit(config);
            services.AddDatabase<DatabaseContext>(config);
            services.AddCustomServices();
            return services;
        }

        private static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<AuthService>();
            services.AddScoped<RoleService>();
            services.AddScoped<UserService>();
            services.AddSingleton<BraintreeService>();
            return services;
        }
    }
}
