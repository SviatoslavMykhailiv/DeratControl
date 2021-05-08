using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations {
  public class FieldConfiguration : BaseEntityTypeConfiguration<Field> {
    public override void Configure(EntityTypeBuilder<Field> builder) {
      base.Configure(builder);
      builder.Property(c => c.TrapId).HasColumnName("TrapFID");
    }
  }
}
