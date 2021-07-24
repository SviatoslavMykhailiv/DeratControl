using System.Collections.Generic;

namespace Application.Points.Queries.PointReviewHistory
{
    public class PointReviewHistoryRecord
    {
        public string ReviewDate { get; init; }
        public string Employee { get; init; }
        public IReadOnlyCollection<PointRecord> Records { get; init; }
    }
}
