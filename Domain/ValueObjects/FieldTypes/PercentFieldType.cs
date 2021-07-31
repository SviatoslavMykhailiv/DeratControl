using Domain.Entities;

namespace Domain.ValueObjects.FieldTypes
{
    public class PercentFieldType : FieldType
    {
        public override byte Id => 4;

        public override string AdjustValue(Field field, string inputValue)
        {
            if (string.IsNullOrWhiteSpace(inputValue))
                return string.Empty;

            if (int.TryParse(inputValue, out var step) && step % field.PercentStep == 0)
                return inputValue;

            return "0";
        }

        public override string ToPrintFormat(Field field, string inputValue) => $"{inputValue}%";
    }
}
