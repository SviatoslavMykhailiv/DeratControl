using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations {
  public class PointReviewConfiguration : BaseEntityTypeConfiguration<PointReview> {
    public override void Configure(EntityTypeBuilder<PointReview> builder) {
      base.Configure(builder);

      builder.Property(c => c.ErrandId).HasColumnName("ErrandFID");
      builder.Property(c => c.PointId).HasColumnName("PointFID");

      builder.HasMany(p => p.Records).WithOne(r => r.PointReview).OnDelete(DeleteBehavior.Cascade);
    }
  }
}
