using Domain.Entities;
using System.Collections.Generic;

namespace Domain.ValueObjects.FieldTypes
{
    public abstract class FieldType
    {
        public static readonly StringFieldType StringFieldType = new StringFieldType();
        public static readonly NumericFieldType NumericFieldType = new NumericFieldType();
        public static readonly OptionFieldType OptionFieldType = new OptionFieldType();
        public static readonly BooleanFieldType BooleanFieldType = new BooleanFieldType();
        public static readonly PercentFieldType PercentFieldType = new PercentFieldType();

        private static readonly Dictionary<byte, FieldType> types = new Dictionary<byte, FieldType> {
            {0,  StringFieldType},
            {1,  NumericFieldType},
            {2,  OptionFieldType},
            {3,  BooleanFieldType},
            {4,  PercentFieldType},
        };

        public static FieldType ToFieldType(byte id)
        {
            return types[id];
        }

        public abstract byte Id { get; }

        public virtual string AdjustValue(Field field, string inputValue) => inputValue;
        public virtual string ToPrintFormat(Field field, string inputValue) => inputValue;

        public static implicit operator FieldType(byte value) => types[value];
        public static implicit operator byte(FieldType value) => value.Id;
    }
}
