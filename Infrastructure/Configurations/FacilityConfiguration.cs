using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace Infrastructure.Configurations {
  public class FacilityConfiguration : BaseEntityTypeConfiguration<Facility> {
    public override void Configure(EntityTypeBuilder<Facility> builder) {
      base.Configure(builder);

      builder.Property(c => c.ProviderId).HasColumnName("ProviderFID");

      builder.HasMany(c => (ICollection<ApplicationUser>)c.Users).WithOne(c => c.Facility).HasForeignKey(c => c.FacilityId);

      builder.HasMany(c => c.Perimeters).WithOne(p => p.Facility).OnDelete(DeleteBehavior.Cascade);

      builder.HasOne(e => (ApplicationUser)e.Provider);
    }
  }
}
