using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Configurations
{
    public class FieldConfiguration : BaseEntityTypeConfiguration<Field>
    {
        public override void Configure(EntityTypeBuilder<Field> builder)
        {
            base.Configure(builder);
            builder.Property(c => c.TrapId).HasColumnName("TrapFID");

            builder.Property(c => c.OptionList)
                .HasConversion(c => JsonConvert.SerializeObject(c), c => JsonConvert.DeserializeObject<IEnumerable<Option>>(c) ?? Enumerable.Empty<Option>());
        }
    }
}
