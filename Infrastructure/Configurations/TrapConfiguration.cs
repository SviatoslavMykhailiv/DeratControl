using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class TrapConfiguration : BaseEntityTypeConfiguration<Trap>
    {
        public override void Configure(EntityTypeBuilder<Trap> builder)
        {
            base.Configure(builder);

            builder.Property(c => c.ProviderId).HasColumnName("ProviderFID");

            builder.HasMany(s => s.Fields)
              .WithOne(f => f.Trap)
              .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => (ApplicationUser)e.Provider);
        }
    }
}
