using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations {
  public class PerimeterConfiguration : BaseEntityTypeConfiguration<Perimeter> {
    public override void Configure(EntityTypeBuilder<Perimeter> builder) {
      base.Configure(builder);

      builder.Property(c => c.FacilityId).HasColumnName("FacilityFID");

      builder.HasMany(p => p.Points).WithOne(p => p.Perimeter).OnDelete(DeleteBehavior.Cascade);
    }
  }
}
