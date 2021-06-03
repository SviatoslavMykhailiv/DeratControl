using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class CompletedErrandConfiguration : BaseEntityTypeConfiguration<CompletedErrand>
    {
        public override void Configure(EntityTypeBuilder<CompletedErrand> builder)
        {
            base.Configure(builder);

            builder.HasOne(e => (ApplicationUser)e.Employee);
            builder.HasOne(e => (ApplicationUser)e.Provider);
        }
    }
}
