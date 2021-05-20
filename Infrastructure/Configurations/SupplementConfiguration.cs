using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations {
  public class SupplementConfiguration : BaseEntityTypeConfiguration<Supplement> {
    public override void Configure(EntityTypeBuilder<Supplement> builder) {
      base.Configure(builder);

      builder.HasOne(e => (ApplicationUser)e.Provider);

      builder.Property(c => c.ProviderId).HasColumnName("ProviderFID");
    }
  }
}
