using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations {
  public class UserConfiguration : BaseEntityTypeConfiguration<ApplicationUser> {
    public override void Configure(EntityTypeBuilder<ApplicationUser> builder) {
      base.Configure(builder);
      builder.Property(c => c.FacilityId).HasColumnName("FacilityFID");
    }
  }
}
