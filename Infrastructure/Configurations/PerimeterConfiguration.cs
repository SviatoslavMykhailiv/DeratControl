using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations {
  public class PerimeterConfiguration : BaseEntityTypeConfiguration<Perimeter> {
    public override void Configure(EntityTypeBuilder<Perimeter> builder) {
      base.Configure(builder);

      builder.Property(c => c.FacilityId).HasColumnName("FacilityFID");
      builder.Property(c => c.PerimeterId).HasColumnName("PerimeterID");
    }
  }
}
