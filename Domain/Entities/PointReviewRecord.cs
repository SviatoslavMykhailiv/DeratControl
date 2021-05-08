using Domain.Common;
using Domain.Enums;
using System;
using System.Linq;

namespace Domain.Entities {
  public class PointReviewRecord : AuditableEntity {

    private string value;

    public Guid PointReviewId { get; init; }
    public Guid SupplementFieldId { get; init; }

    public string Value {
      get => value;

      set {
        if (string.IsNullOrWhiteSpace(value))
          return;

        if (Field.FieldType == FieldType.Numeric && int.TryParse(value, out _) == false)
          throw new InvalidOperationException("Value must be a number.");
        
        if (Field.FieldType == FieldType.Option) {
          var options = Field.OptionList.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(c => c.Trim());

          if (options.Contains(value) == false)
            throw new InvalidOperationException("Value does't exist in option list.");
        }

        this.value = value;
      }
    }

    public PointReview PointReview { get; init; }
    public Field Field { get; init; }
  }
}
