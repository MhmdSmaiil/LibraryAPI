using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
//using Library.Resources.Middleware;
//using Microsoft.Extensions.Logging;
//using Serilog;
//using Microsoft.OpenApi.Models;
//using Library.Resources.Intefraces.Helpers;
//using Library.Resources.Helpers;

namespace Library.Resources.Extensions
{
    public static class StartupExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {

            //services.AddSingleton<SharedResources>();
            //services.AddTransient<CultureMiddleware>();
            //services.AddTransient<ExceptionMiddleware>();

            //#region register services
            //services.AddScoped<IEmailSenderHelper, EmailSenderHelper>();
            //#endregion

            return services;
        }
        public static IServiceCollection AddDbLogger(this IServiceCollection services, string connectionString)
        {
            //Log.Logger = new LoggerConfiguration()
            //    .MinimumLevel.Error()
            //    .WriteTo.PostgreSQL(
            //    connectionString: connectionString,
            //    tableName: "Logs",
            //    needAutoCreateTable: true)
            //    .CreateLogger();

            //services.AddLogging(loggingBuilder =>
            //{
            //    loggingBuilder.ClearProviders();
            //    loggingBuilder.AddConsole();
            //    loggingBuilder.AddSerilog();
            //});


            return services;
        }
        public static IServiceCollection SetUpSwagger(this IServiceCollection services)
        {
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo
            //    {
            //        Version = "v1",
            //        Title = "Library API",
            //        Description = "A list of all apis used in Library App",
            //    });

            //    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            //    {
            //        Description = @"JWT Authorization header using the Bearer scheme. <br>
            //          Enter 'Bearer' [space] and then your token in the text input below.<br>
            //          Example: 'Bearer 12345abcdef'",
            //        Name = "Authorization",
            //        In = ParameterLocation.Header,
            //        Type = SecuritySchemeType.ApiKey,
            //        Scheme = "Bearer"
            //    });

            //    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            //    {
            //        {
            //            new OpenApiSecurityScheme
            //            {
            //                Reference = new OpenApiReference
            //                {
            //                    Type = ReferenceType.SecurityScheme,
            //                    Id = "Bearer"
            //                },
            //                Scheme = "oauth2",
            //                Name = "Bearer",
            //                In = ParameterLocation.Header,
            //            },
            //            new List<string>()
            //        }
            //    });
            //});

            return services;
        }
        public static IApplicationBuilder AddExceptionHandler(this IApplicationBuilder app)
        {
            //app.UseMiddleware<ExceptionMiddleware>();

            return app;
        }
    }
}
