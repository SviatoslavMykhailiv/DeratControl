using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities
{
    public class CompletedErrand : AuditableEntity
    {
        private readonly HashSet<CompletedPointReview> pointReviewHistory;

        public CompletedErrand() { }

        public CompletedErrand(
            Errand errand, 
            DateTime completeDate, 
            string report, 
            IEnumerable<CompletedPointReview> completedPointReviews)
        {
            CompleteDate = completeDate;
            DueDate = errand.DueDate;
            Description = errand.Description;
            OnDemand = errand.OnDemand;
            Facility = errand.Facility.GetInfo();
            Employee = errand.Employee.GetFullName();
            Provider = errand.Provider.GetFullName();
            Report = report;
            pointReviewHistory = completedPointReviews.ToHashSet();
        }

        public DateTime CompleteDate { get; init; }
        public DateTime DueDate { get; init; }
        public string Description { get; init; }
        public bool OnDemand { get; init; }
        public string Facility { get; init; }
        public string Employee { get; init; }
        public string Provider { get; init; }
        public string Report { get; init; }
        public IEnumerable<CompletedPointReview> PointReviewHistory => pointReviewHistory;
    }
}
