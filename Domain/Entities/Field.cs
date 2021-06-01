using Domain.Common;
using Domain.Enums;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities
{
    public class Field : AuditableEntity
    {
        public static readonly HashSet<FieldType> OptionTypes = new HashSet<FieldType>
        {
            FieldType.Option,
            FieldType.Percent
        };

        public bool IsOptionType => OptionTypes.Contains(FieldType);

        public Guid TrapId { get; init; }
        public string FieldName { get; set; }
        public int Order { get; set; }

        public FieldType FieldType { get; set; }

        public Trap Trap { get; init; }

        public IEnumerable<Option> OptionList { get; set; } = new HashSet<Option>();

        public bool ContainsOption(string option)
        {
            return OptionList.Any(c => c.Value == option);
        }
    }
}
