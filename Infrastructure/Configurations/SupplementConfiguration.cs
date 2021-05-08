using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations {
  public class SupplementConfiguration : BaseEntityTypeConfiguration<Supplement> {
    public override void Configure(EntityTypeBuilder<Supplement> builder) {
      base.Configure(builder);
    }
  }
}
