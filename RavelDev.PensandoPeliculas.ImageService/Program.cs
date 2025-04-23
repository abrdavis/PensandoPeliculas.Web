using RavelDev.PensandoPeliculas.Core.Configs;
using RavelDev.PensandoPeliculas.ImageService.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using RavelDev.PensandoPeliculas.Core.Models.Config;
using RavelDev.Core.Interfaces;
using RavelDev.Core.Repo;
using RavelDev.PensandoPeliculas.Users;
using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RavelDev.PensandoPeliculas.Entity;
using Azure.Storage.Blobs;
using Azure.Identity;
using RavelDev.PensandoPeliculas.ImageService.API;
using static RavelDev.PensandoPeliculas.Core.Utility.ImageConstants;
using RavelDev.PensandoPeliculas.Core.Models;

var builder = WebApplication.CreateBuilder(args);
var keyVaultName = builder.Configuration.GetSection("AzureKeys:KeyVaultName").Get<string>();
Uri vaultUri = new Uri($"https://{keyVaultName}.vault.azure.net/");

var credentails = new DefaultAzureCredential();

builder.Configuration.AddAzureKeyVault(vaultUri, credentails);


var blobStorageConfig = builder.Configuration.GetSection("AzureBlobStorage").Get<AzureBlobStorageConfig>();

if(blobStorageConfig == null)
{
    Console.WriteLine("Blob storage config data not found");
    return;
}

var blobServiceClient = new BlobServiceClient(
        new Uri(blobStorageConfig.BlobStorageUrl),
        new DefaultAzureCredential());

var  containerClient = blobServiceClient.GetBlobContainerClient(blobStorageConfig.DefaultContainer);
var jwtKey = builder.Configuration["pensando-api-jwt-key"];
var jwtIssuer = builder.Configuration["pensando-api-jwt-issuer"];

var jwtSettings = new JwtSettings(jwtKey!, jwtIssuer!);
var key = Encoding.UTF8.GetBytes(jwtSettings.Key);
builder.Services.AddAuthorization();

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
            .AddJwtBearer(x =>
            {
                x.IncludeErrorDetails = true;
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidIssuer = jwtSettings.Issuer,
                    ClockSkew = TimeSpan.Zero
                };
            });


var services = builder.Services;

var imageUploadConfig = builder.Configuration.GetSection("ImageUploadConfig");
var connectionString = builder.Configuration["pensando-local-cs"];

services.AddScoped<ImageWriter>();
services.Configure<ImageUploadConfig>(imageUploadConfig);

builder.Services.AddTransient<IDbConnection>((sp) => new SqlConnection(connectionString));
builder.Services.AddSingleton<IRepositoryConfig>(new RepositoryConfig(connectionString));

builder.Services.AddScoped<ImageApi>();

builder.Services.AddDbContext<PeliculaDbContext>(options =>
options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();


app.MapPost("/upload", async (HttpContext context, ImageApi imageApi) =>
{
    ImageUploadResult result = new ImageUploadResult();
    try
    {
        var form = await context.Request.ReadFormAsync();
        var files = form.Files;

        if (!form.ContainsKey("imageTypeId"))
        {
            return Results.Json(new { success = false, noFileType = true });
        }

        var imageType =  int.Parse(form["imageTypeId"].ToString());
        var imageFile = files.Any() && files.Count > 0 ? files[0] : null;

        if (imageFile == null)
        {
            return Results.Json(new { success = false, noImageFound = true });
        }

        using var imageStream = imageFile.OpenReadStream();
        var fileName = $"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}_{imageFile.FileName}";
        switch (imageType)
        {
            case ImageTypes.Poster:
                result = await imageApi.UploadMoviePoster(containerClient, imageStream, fileName);
                break;
            case ImageTypes.ReviewHeader:
                result = await imageApi.UploadReviewHeader(containerClient, imageStream, fileName);
                break;
            default:
                Console.WriteLine($"Unknown image type provided - value was {imageType}");
                break;
        }
       
        return Results.Json(result);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return Results.Json(result);
    }

}).RequireAuthorization();



app.Run();
