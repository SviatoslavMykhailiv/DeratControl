using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations {
  public class PointReviewRecordConfiguration : BaseEntityTypeConfiguration<PointReviewRecord> {
    public override void Configure(EntityTypeBuilder<PointReviewRecord> builder) {
      base.Configure(builder);

      builder.Property(c => c.PointReviewId).HasColumnName("PointReviewFID");
      builder.Property(c => c.SupplementFieldId).HasColumnName("SupplementFieldID");
    }
  }
}
