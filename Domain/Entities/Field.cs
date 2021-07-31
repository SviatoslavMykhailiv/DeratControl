using Domain.Common;
using Domain.ValueObjects;
using Domain.ValueObjects.FieldTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities
{
    public class Field : AuditableEntity
    {
        public Guid TrapId { get; init; }
        public string FieldName { get; set; }
        public int Order { get; set; }
        public bool AdminEditable { get; set; }

        public FieldType FieldType { get; set; }

        public Trap Trap { get; init; }

        public IEnumerable<Option> OptionList { get; set; } = new HashSet<Option>();

        public int PercentStep { get; set; }

        public bool ContainsOption(string option)
        {
            return OptionList.Any(c => c.Name == option);
        }

        public string ToPrintFormat(string value) => FieldType.ToPrintFormat(this, value);

        public string AdjustValue(string value) => FieldType.AdjustValue(this, value);
    }
}
