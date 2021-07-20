using Domain.Enums;
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
                result.AddRecord(field, valueDictionary.GetValueOrDefault(field.Id));
            }

            return result;
        }
    }
}
