using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations {
  public class DefaultFacilityConfiguration : BaseEntityTypeConfiguration<DefaultFacility> {
    public override void Configure(EntityTypeBuilder<DefaultFacility> builder) {
      base.Configure(builder);

      builder.HasKey(c => new { c.FacilityId, c.UserId });

      builder.Property(c => c.FacilityId).HasColumnName("FacilityFID");
      builder.Property(c => c.UserId).HasColumnName("UserFID");

      builder.HasOne(c => (ApplicationUser)c.User).WithMany(u => u.DefaultFacilities).HasForeignKey(c => c.UserId);
    }
  }
}
