using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Domain.ValueObjects.FieldTypes
{
    public class BooleanFieldType : FieldType
    {
        private static readonly HashSet<string> trueValues = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase) { "true", "1" };

        public override byte Id => 3;

        public override string AdjustValue(Field field, string inputValue)
        {
            if (trueValues.Contains(inputValue))
                return "1";

            return "0";
        }

        public override string ToPrintFormat(Field field, string inputValue)
        {
            if (string.IsNullOrWhiteSpace(inputValue))
                return "Ні";

            return Convert.ToBoolean(int.Parse(inputValue)) ? "Так" : "Ні";
        }
    }
}
