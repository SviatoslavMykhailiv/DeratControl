using Domain.Entities;

namespace Domain.ValueObjects.FieldTypes
{
    public class NumericFieldType : FieldType
    {
        public override byte Id => 1;

        public override string AdjustValue(Field field, string inputValue)
        {
            if (int.TryParse(inputValue, out int result))
                return result.ToString();
            
            return "0";
        }
    }
}
