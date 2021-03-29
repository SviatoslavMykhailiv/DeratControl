using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Infrastructure {
  public abstract class DesignTimeDbContextFactoryBase<TContext> : IDesignTimeDbContextFactory<TContext> where TContext : DbContext {
    private const string ConnectionStringName = "DeratControlDatabase";

    public TContext CreateDbContext(string[] args) {
      var basePath = Directory.GetCurrentDirectory() + string.Format("{0}..{0}API", Path.DirectorySeparatorChar);
      return Create(basePath);
    }

    protected abstract TContext CreateNewInstance(DbContextOptions<TContext> options);

    private TContext Create(string basePath) {

      var configuration = new ConfigurationBuilder()
        .SetBasePath(basePath)
          .AddJsonFile("appsettings.json")
          .Build();

      var optionsBuilder = new DbContextOptionsBuilder<TContext>();

      optionsBuilder.UseNpgsql(configuration.GetConnectionString(ConnectionStringName));

      return CreateNewInstance(optionsBuilder.Options);
    }
  }
}
