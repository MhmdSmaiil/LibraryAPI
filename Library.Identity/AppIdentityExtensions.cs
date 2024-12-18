using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Library.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Library.Identity.Services;
using Library.Identity.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Library.Identity
{
    public static class AppIdentityExtensions
    {
        public static IServiceCollection ConfigureIdentityDependencies(this IServiceCollection services)
        {
            services.AddScoped<IAppIdentityService, AppIdentityService>();
            services.AddScoped<IAppIdentityDbContextSeed, AppIdentityDbContextSeed>();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            return services;
        }
        public static IServiceCollection AddJWTAuthentication(this IServiceCollection services, string jwtSecret, string audience, string authority)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = authority,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = 401;
                            context.Response.Headers.Append("WWW-Authenticate", "Bearer");
                            return Task.CompletedTask;
                        }
                    };
                });

            return services;

        }
        public static IServiceCollection AddRolesPolicy(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Roles.Admin, policy =>
                {
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole(Roles.Admin);
                });
                options.AddPolicy(Roles.User, policy =>
                {
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole(Roles.User);
                });
            });

            return services;
        }
        public static async Task<IApplicationBuilder> SeedData(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dbContextSeed = scope.ServiceProvider.GetService<IAppIdentityDbContextSeed>();
                await dbContextSeed.SeedAsync();
            }
            return app;
        }
    }
}
