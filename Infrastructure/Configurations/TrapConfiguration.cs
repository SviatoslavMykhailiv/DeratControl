using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations {
  public class TrapConfiguration : BaseEntityTypeConfiguration<Trap> {
    public override void Configure(EntityTypeBuilder<Trap> builder) {
      base.Configure(builder);

      builder.Property(c => c.TrapId).HasColumnName("TrapID");
    }
  }
}
