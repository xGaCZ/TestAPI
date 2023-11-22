using FluentValidation;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using System.Reflection;
using System.Text;
using TestNetAPI;
using TestNetAPI.Entities;
using TestNetAPI.Middleware;
using TestNetAPI.Models;
using TestNetAPI.Models.Validators;
using TestNetAPI.Services;

var builder = WebApplication.CreateBuilder(args);
var authenticationSettings = new AuthenticationSettings();
builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);
builder.Services.AddSingleton(authenticationSettings);
builder.Services.AddAuthentication(option =>
{
    option.DefaultScheme = "Bearer";
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = authenticationSettings.JwtIssuer,
        ValidAudience = authenticationSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
    };
});

builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Host.UseNLog();
// Add services to the container.
builder.Services.AddDbContext<NetDbContext>
    (options => options.UseSqlServer(builder.Configuration.GetConnectionString("ContactDbConnection")));

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IDetailService, DetailService>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<Seeder>();
builder.Services.AddScoped<IValidator<RegisterUserDto>,RegisterUserDtoVaildator>();
builder.Services.AddScoped<IPasswordHasher<User>,PasswordHasher<User>>();
builder.Services.AddScoped<ErrorHandlingMiddleware>();


var app = builder.Build();
var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<Seeder>();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseAuthentication();
seeder.Seed(); //seeduje dane 

app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "NetAPI");
});

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
