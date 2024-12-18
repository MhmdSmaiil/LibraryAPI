using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System;
using Library.Data.Interfaces.Repositories;
using Library.Data.Repositories;
using Library.Data.Interfaces.Services;
using Library.Data.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Library.Data
{
    public static class AppDbConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRepos(this IServiceCollection services)
        {
            services.AddScoped<IAppUserRepository, AppUserRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAppUserService, AppUserService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }

        public static IApplicationBuilder DbContextMigrate(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dataContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var connection = dataContext.Database.GetDbConnection();
                try
                {
                    if (connection.State == System.Data.ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    if (!dataContext.Database.GetDbConnection().GetSchema("Tables").Rows
                      .OfType<DataRow>()
                      .Any(row => (string)row["TABLE_NAME"] == "AspNetRoles"))
                    {
                        dataContext.Database.Migrate();
                    }
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close(); // Ensure connection is closed
                    }
                }
            }
            return app;
        }
    }
}