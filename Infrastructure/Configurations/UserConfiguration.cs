using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations {
  public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser> {
    public void Configure(EntityTypeBuilder<ApplicationUser> builder) {
      builder.HasOne(e => (ApplicationUser)e.Provider);
      builder.Property(c => c.ProviderId).HasColumnName("ProviderFID");

      builder.OwnsOne(c => c.Device, o => {
        o.Property(p => p.DeviceIdentifier).HasColumnName("DeviceIdentifier");
        o.Property(p => p.DeviceType).HasColumnName("DeviceType");
      });
    }
  }
}
