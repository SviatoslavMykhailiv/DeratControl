using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations {
  public class PointConfiguration : BaseEntityTypeConfiguration<Point> {
    public override void Configure(EntityTypeBuilder<Point> builder) {
      base.Configure(builder);

      builder.Property(c => c.PointId).HasColumnName("PointID");
      builder.Property(c => c.PerimeterId).HasColumnName("PerimeterFID");
      builder.Property(c => c.TrapId).HasColumnName("TrapFID");
      builder.Property(c => c.SupplementId).HasColumnName("SupplementFID");
    }
  }
}
