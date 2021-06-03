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
            Facility = errand.Facility;
            Employee = errand.Employee;
            Provider = errand.Provider;
            Report = report;
            FacilityId = errand.FacilityId;
            EmployeeId = errand.EmployeeId;
            pointReviewHistory = completedPointReviews.ToHashSet();
        }

        public DateTime CompleteDate { get; init; }
        public DateTime DueDate { get; init; }
        public string Description { get; init; }
        public bool OnDemand { get; init; }

        public Guid FacilityId { get; init; }
        public Facility Facility { get; init; }
        
        public Guid EmployeeId { get; init; }
        public IUser Employee { get; init; }
        
        public Guid ProviderId { get; init; }
        public IUser Provider { get; init; }
        
        public string Report { get; init; }
        public IEnumerable<CompletedPointReview> PointReviewHistory => pointReviewHistory;
    }
}
