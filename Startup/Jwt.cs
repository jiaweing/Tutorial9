using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Web.Models;

namespace Web.Startup
{
    public static class Jwt
    {
        public static IServiceCollection AddJwt(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.Name = config.GetValue("Web:App:Name", "myapp");
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.LoginPath = new PathString("/Auth/Login");
                options.AccessDeniedPath = new PathString("/Denied");
                options.LogoutPath = new PathString("/Auth/Logout");
                options.ClaimsIssuer = config.GetValue("Web:Jwt:Issuer", "example.com");
                options.ExpireTimeSpan = TimeSpan.FromMinutes(config.GetValue("Web:Cookies:ExpireTimeSpanMinutes", 60));
                options.SlidingExpiration = config.GetValue("Web:Cookies:SlidingExpiration", true);
                options.Events.OnRedirectToLogin = (context) =>
                {
                    context.Response.Redirect(context.RedirectUri);
                    return Task.CompletedTask;
                };
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config.GetValue<string>("Web:Jwt:Issuer") ?? "example.com",
                    ValidAudience = config.GetValue<string>("Web:Jwt:Audience") ?? "example.com",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetValue<string>("Web:Jwt:Key") ?? "supersecretkey"))
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(CustomRoles.Admin, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
                    policy.RequireRole(CustomRoles.Admin);
                });

                options.AddPolicy(CustomRoles.User, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
                    policy.RequireRole(CustomRoles.User);
                });

                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                       .AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
                       .RequireAuthenticatedUser()
                       .Build();
            });

            return services;
        }
    }
}
