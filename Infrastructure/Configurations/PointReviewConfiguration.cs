using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class PointReviewConfiguration : IEntityTypeConfiguration<PointReview>
    {
        public void Configure(EntityTypeBuilder<PointReview> builder)
        {
            builder.ToTable(typeof(PointReview).Name);
            builder.HasKey(c => new { c.ErrandId, c.PointId });

            builder.Property(c => c.ErrandId).HasColumnName("ErrandFID");
            builder.Property(c => c.PointId).HasColumnName("PointFID");
        }
    }
}
