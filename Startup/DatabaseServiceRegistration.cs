using Microsoft.EntityFrameworkCore;

namespace Web.Startup
{
    public static class DatabaseServiceRegistration
    {
        public static IServiceCollection AddDatabase<T>(this IServiceCollection services, IConfiguration config) where T : DbContext
        {
            string host = config.GetValue<string>("Web:Database:Mysql:Host");
            string port = config.GetValue<string>("Web:Database:Mysql:Port");
            string database = config.GetValue<string>("Web:Database:Mysql:Database");
            string username = config.GetValue<string>("Web:Database:Mysql:Username");
            string password = config.GetValue<string>("Web:Database:Mysql:Password");
            string connectionString = $"server={host};port={port};database={database};user={username};password={password};";
            services.AddDbContext<T>(delegate (DbContextOptionsBuilder options)
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)).UseSnakeCaseNamingConvention();
            });

            return services;
        }
    }
}
