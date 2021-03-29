using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations {
  public class ErrandConfiguration : BaseEntityTypeConfiguration<Errand> {
    public override void Configure(EntityTypeBuilder<Errand> builder) {
      base.Configure(builder);

      builder.Property(c => c.ErrandId).HasColumnName("ErrandID");
      builder.Property(c => c.FacilityId).HasColumnName("FacilityFID");
      builder.Property(c => c.EmployeeId).HasColumnName("EmployeeFID");

      builder
        .HasOne(e => (ApplicationUser)e.Employee)
        .WithMany(e => e.Errands)
        .HasForeignKey(e => e.EmployeeId);
    }
  }
}
