using Azure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;

using Microsoft.IdentityModel.Tokens;
using RavelDev.Core.Interfaces;
using RavelDev.Core.Repo;
using RavelDev.PensadoPeliculas.Images.Models;
using RavelDev.PensadoPeliculas.Images.Service;
using RavelDev.PensandoPeliculas.Core.API;
using RavelDev.PensandoPeliculas.Core.DataAccess;
using RavelDev.PensandoPeliculas.Core.Models.Config;
using RavelDev.PensandoPeliculas.Entity;
using RavelDev.PensandoPeliculas.Users;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using static RavelDev.PensandoPeliculas.WebApi.Utility.Authentication.AuthConstants;


var builder = WebApplication.CreateBuilder(args);

var keyVaultName = builder.Configuration.GetSection("AzureKeys:KeyVaultName").Get<string>();
Uri vaultUri = new Uri($"https://{keyVaultName}.vault.azure.net/");

var credentails = new DefaultAzureCredential();

builder.Configuration.AddAzureKeyVault(vaultUri, credentails);


var services = builder.Services;
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("https://localhost:5173",
                              "https://witty-forest-06d3ecb0f.6.azurestaticapps.net",
                              "http://127.0.0.1:5173",
                              "http://localhost:5173",
                              "https://postmeta.org",
                              "https://dev.local:8080",
                             "https://localhost:8080"
                             )
                              .AllowAnyHeader()
                             .AllowCredentials();

                      });
});



var imageServiceUrl = builder.Configuration.GetSection("ServiceUrls:ImageServiceUrl").Get<string>()!;
builder.Services.AddSingleton<ImageServiceConfig>( new ImageServiceConfig(imageServiceUrl));


var dbConnectionString = builder.Configuration["pensando-local-cs"];

// Inject IDbConnection, with implementation from SqlConnection class.
builder.Services.AddTransient<IDbConnection>((sp) => new SqlConnection(dbConnectionString));
builder.Services.AddSingleton<IRepositoryConfig>(new RepositoryConfig(dbConnectionString));
builder.Services.AddScoped<IUserManager, UserManager>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ImageUploadService>();
builder.Services.AddScoped<ReviewApi>();
builder.Services.AddScoped<ReviewRepository>();
builder.Services.AddScoped<TitleRepository>();
builder.Services.AddScoped<MetaDataRepository>();
builder.Services.AddScoped<MetaDataApi>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<PeliculaDbContext>(options =>
options.UseMySql(
        dbConnectionString, ServerVersion.AutoDetect(dbConnectionString)));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


var jwtKey = builder.Configuration["pensando-api-jwt-key"];
var jwtIssuer = builder.Configuration["pensando-api-jwt-issuer"];

var jwtSettings = new JwtSettings(jwtKey!, jwtIssuer!);

services.AddSingleton<JwtSettings>(jwtSettings);

var jwtEncoded = Encoding.UTF8.GetBytes(jwtKey);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Cookies.ContainsKey(AuthCookieNames.AcecssToken))
                        {
                            context.Token = context.Request.Cookies[AuthCookieNames.AcecssToken];
                        }
                        return Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(jwtEncoded),
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidIssuer = jwtSettings.Issuer,
                    ClockSkew = TimeSpan.Zero
                };
            });


var app = builder.Build();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseDeveloperExceptionPage();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<JwtMiddleware>();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
