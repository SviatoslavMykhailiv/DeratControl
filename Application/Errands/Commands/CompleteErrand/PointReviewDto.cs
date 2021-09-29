using System;
using System.Collections.Generic;

namespace Application.Errands.Commands.CompleteErrand
{
    public class PointReviewDto
    {
        public Guid PointId { get; init; }

        public IReadOnlyCollection<PointReviewRecordDto> Records { get; init; } = new List<PointReviewRecordDto>();

        public Dictionary<Guid, string> GetValueList()
        {
            var result = new Dictionary<Guid, string>();

            foreach (var record in Records)
                result.Add(record.FieldId, record.Value);

            return result;
        }
    }
}
