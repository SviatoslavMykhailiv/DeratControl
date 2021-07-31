using Domain.Entities;
using System.Linq;

namespace Domain.ValueObjects.FieldTypes
{
    public class OptionFieldType : FieldType
    {
        public override byte Id => 2;

        public override string AdjustValue(Field field, string inputValue)
        {
            if(field.OptionList.Any(o => o.Name.Equals(inputValue))) 
            {
                return inputValue;
            }

            return string.Empty;
        }

        public override string ToPrintFormat(Field field, string inputValue)
        {
            return field.OptionList.FirstOrDefault(opt => opt.Name.Equals(inputValue))?.Description ?? string.Empty;
        }
    }
}
