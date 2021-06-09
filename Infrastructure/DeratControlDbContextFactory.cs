using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class DeratControlDbContextFactory : DesignTimeDbContextFactoryBase<DeratControlDbContext>
    {
        protected override DeratControlDbContext CreateNewInstance(DbContextOptions<DeratControlDbContext> options)
        {
            return new DeratControlDbContext(options, null, null);
        }
    }
}
