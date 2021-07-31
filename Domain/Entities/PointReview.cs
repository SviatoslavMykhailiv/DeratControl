using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class PointReview
    {
        public PointReview() { }

        public PointReview(Point point)
        {
            Point = point;
        }

        public Guid ErrandId { get; init; }
        public Guid PointId { get; init; }
        public Errand Errand { get; init; }
        public Point Point { get; init; }

        public CompletedPointReview Complete(string report, Dictionary<Guid, string> valueDictionary) 
        {
            var result = new CompletedPointReview(Point, report);

            foreach (var field in Point.Trap.Fields)
            {
                var value = valueDictionary.GetValueOrDefault(field.Id);
                result.AddRecord(field, value);
                Point.SetFieldValue(field.Id, value);
            }

            return result;
        }
    }
}
