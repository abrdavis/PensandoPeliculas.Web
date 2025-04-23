using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using RavelDev.PensandoPeliculas.Entity;
using RavelDev.PensandoPeliculas.Entity.Models;
using RavelDev.PensandoPeliculas.Entity.Models.MetaData;
using RavelDev.PensandoPeliculas.Entity.Models.Reviews;
using RavelDev.PensandoPeliculas.Entity.Models.Titles;


namespace RavelDev.PensandoPeliculas.Entity
{
    public class PeliculaDbContext : DbContext
    {
        public PeliculaDbContext(DbContextOptions<PeliculaDbContext> options) : base(options)
        {

        }

        public DbSet<Title> Titles { get; set;}
        public DbSet<Role> Roles { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<TitleGenre> TitleGenres { get; set; }


    }

}

public class PeliculaDbContextFactory :
        IDesignTimeDbContextFactory<PeliculaDbContext>
{
    public PeliculaDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddUserSecrets<PeliculaDbContextFactory>()
            .Build();
        var builder = new DbContextOptionsBuilder<PeliculaDbContext>();
        var keyVaultName = configuration.GetSection("AzureKeys:KeyVaultName").Get<string>();

        var kvUri = $"https://{keyVaultName}.vault.azure.net";

        var kvClient = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());

        var connectionString = kvClient.GetSecret("local-db-cs").Value?.Value ?? string.Empty;

        builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        return new PeliculaDbContext(builder.Options);
    }
}
