using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class PointFieldValueConfiguration : BaseEntityTypeConfiguration<PointFieldValue>
    {
        public override void Configure(EntityTypeBuilder<PointFieldValue> builder)
        {
            base.Configure(builder);
            builder.Property(c => c.FieldId).HasColumnName("FieldFID");
            builder.Property(c => c.PointId).HasColumnName("PointFID");

            builder.HasOne(c => c.Field);
            builder.HasOne(c => c.Point).WithMany(p => p.Values).HasForeignKey(p => p.PointId);
        }
    }
}
