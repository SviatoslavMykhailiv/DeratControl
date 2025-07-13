using Domain.Entities;
using Domain.ValueObjects;
using Domain.ValueObjects.FieldTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Infrastructure.Configurations
{
    public class FieldConfiguration : BaseEntityTypeConfiguration<Field>
    {
        private readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions();

        public override void Configure(EntityTypeBuilder<Field> builder)
        {
            base.Configure(builder);
            builder.Property(c => c.TrapId).HasColumnName("TrapFID");

            builder.Property(c => c.OptionList)
                .HasConversion(c => JsonSerializer.Serialize(c, jsonSerializerOptions), c => JsonSerializer.Deserialize<IEnumerable<Option>>(c, jsonSerializerOptions) ?? Enumerable.Empty<Option>());

            builder.Property(c => c.FieldType)
                .HasConversion(c => c.Id, c => FieldType.ToFieldType(c));

        }
    }
}
