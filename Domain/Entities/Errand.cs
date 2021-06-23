using Domain.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Domain.Entities
{
    /// <summary>
    /// Represents an errand.
    /// </summary>
    public class Errand : AuditableEntity
    {
        private string description;
        private readonly HashSet<PointReview> points = new HashSet<PointReview>();

        /// <summary>
        /// Initializes a new instance of <see cref="Errand"/> with Planned status.
        /// </summary>
        public Errand()
        {
            
        }

        public Guid FacilityId { get; init; }
        public Facility Facility { get; set; }

        public Guid EmployeeId { get; set; }
        public IUser Employee { get; set; }

        public Guid ProviderId { get; set; }
        public IUser Provider { get; set; }

        public DateTime OriginalDueDate { get; private set; }
        public DateTime DueDate { get; private set; }

        public string Description
        {
            get => description;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new InvalidOperationException("Description cannot be empty.");

                description = value;
            }
        }

        public string GetManagerSignatureFilePath() => Path.Combine("signatures", $"{Id}-signature.png");

        public bool OnDemand { get; set; }

        public IEnumerable<PointReview> Points => points;

        public bool IsOverdue(DateTime currentDate) => DueDate.Date < currentDate.Date;

        public void SetDueDate(DateTime dueDate)
        {
            DueDate = dueDate.Date;
            OriginalDueDate = dueDate.Date;
        }

        public void MoveDueDate(DateTime currentDate)
        {
            if (IsOverdue(currentDate))
                DueDate = currentDate.Date;
        }

        public int GetOverdueDays(DateTime currentDate)
        {
            var days = (currentDate.Date - DueDate.Date).Days;
            return days < 0 ? 0 : days;
        }

        public CompletedErrand Complete(
            DateTime completeDate, 
            string report, 
            IEnumerable<CompletedPointReview> completedPointReviews)
        {
            return new CompletedErrand(this, completeDate, report, completedPointReviews);
        }

        public void SetPointListForReview(IEnumerable<Guid> selectedPointList)
        {
            var inputPointSet = selectedPointList.ToHashSet();

            var existingPointList = points.ToDictionary(p => p.PointId);

            var removePointList = from pointId in existingPointList.Keys
                                  where inputPointSet.Contains(pointId) == false
                                  select pointId;

            points.RemoveWhere(p => removePointList.Contains(p.PointId));

            var pointList = Facility.Perimeters.SelectMany(p => p.Points).ToDictionary(p => p.Id);

            foreach (var pointId in selectedPointList.Where(id => existingPointList.ContainsKey(id) == false))
            {
                var pointReview = new PointReview(pointList[pointId]);
                points.Add(pointReview);
            }
        }

        public bool IsSecurityCodeValid(string securityCode) => string.Equals(securityCode, Facility.SecurityCode, StringComparison.OrdinalIgnoreCase);
    }
}
