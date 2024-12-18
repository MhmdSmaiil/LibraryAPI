using Library.Data;
using Library.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Library.Identity.Entities;
using Library.Resources.Extensions;
using Library.resources.Middleware;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var jwtSecret = builder.Configuration.GetSection("JwtConfig").GetValue<string>("Secret") ?? string.Empty;
var audience = builder.Configuration.GetSection("JwtConfig").GetValue<string>("Audience") ?? string.Empty;
var authority = builder.Configuration.GetSection("JwtConfig").GetValue<string>("Issuer") ?? string.Empty;
var dbConnection = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("LibraryAPI")));

builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<AppRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddTokenProvider<DataProtectorTokenProvider<AppUser>>(TokenOptions.DefaultProvider);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.SetUpSwagger();
builder.Services.ConfigureServices();
builder.Services.ConfigureIdentityDependencies();
builder.Services.AddJWTAuthentication(jwtSecret, audience, authority);
builder.Services.AddRolesPolicy();
builder.Services.AddDbLogger(dbConnection);
builder.Services.AddRepos();
builder.Services.AddServices();
builder.Services.AddOptions();
builder.Services.AddTransient<ExceptionMiddleware>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowAnyOrigin();
    });
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.DbContextMigrate();
await app.SeedData();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();
app.UseCors();
app.Run();