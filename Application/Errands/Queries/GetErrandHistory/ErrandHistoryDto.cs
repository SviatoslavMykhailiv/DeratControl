using Domain.Entities;
using System;

namespace Application.Errands.Queries.GetErrandHistory
{
    public class ErrandHistoryDto
    {
        public Guid ErrandId { get; init; }
        public string Description { get; init; }
        public string Report { get; init; }
        public string DueDate { get; init; }
        public string CompleteDate { get; init; }
        public string EmployeeName { get; init; }
        public string FacilityName { get; init; }

        public static ErrandHistoryDto Map(CompletedErrand errand)
        {
            return new ErrandHistoryDto
            {
                ErrandId = errand.Id,
                Description = errand.Description,
                Report = errand.Report,
                DueDate = errand.DueDate.ToString("d"),
                CompleteDate = errand.CompleteDate.ToString("d"),
                EmployeeName = errand.Employee.GetFullName(),
                FacilityName = errand.Facility.GetInfo()
            };
        }
    }

    public class PointReviewHistoryDto
    {
        public int Order { get; init; }
        public string TrapName { get; init; }
        public string SupplementName { get; init; }
        public string PerimeterName { get; init; }
        public string Status { get; init; }
    }
}
