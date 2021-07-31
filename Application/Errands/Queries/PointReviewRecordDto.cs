using System;
using Domain.Enums;
using Domain.ValueObjects;

namespace Application.Errands.Queries
{
    public class PointReviewRecordDto
    {
        public Guid FieldId { get; init; }
        public string FieldName { get; init; }
        public byte FieldType { get; init; }
        public Option[] OptionList { get; init; }
        public int PercentStep { get; init; }
        public string Value { get; init; }
    }
}
