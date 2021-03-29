using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations {
  public class SupplementFieldConfiguration : BaseEntityTypeConfiguration<SupplementField> {
    public override void Configure(EntityTypeBuilder<SupplementField> builder) {
      base.Configure(builder);
      builder.Property(c => c.SupplementFieldId).HasColumnName("SupplementFieldFID");
      builder.Property(c => c.SupplementId).HasColumnName("SupplementFID");
    }
  }
}
