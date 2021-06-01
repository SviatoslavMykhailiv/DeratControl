using System;
using System.Collections.Generic;

namespace Application.Errands.Queries
{
    public class PointReviewDto
    {
        public Guid PerimeterId { get; init; }
        public string PerimeterName { get; init; }
        public Guid PointId { get; init; }
        public int Order { get; init; }
        public Guid TrapId { get; init; }
        public string TrapName { get; init; }
        public string TrapColor { get; init; }

        public ICollection<PointReviewRecordDto> Records { get; init; } = new List<PointReviewRecordDto>();
    }
}
