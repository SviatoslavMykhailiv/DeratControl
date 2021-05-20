using Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Application.Errands.Queries.GetErrandHistory {
  public class ErrandHistoryDto {
    public string Description { get; init; }
    public string Report { get; init; }
    public string DueDate { get; init; }
    public string CompleteDate { get; init; }
    public string EmployeeName { get; init; }
    public string FacilityName { get; init; }
    public ICollection<PointReviewHistoryDto> Points { get; init; } = new List<PointReviewHistoryDto>();

    public static ErrandHistoryDto Map(Errand errand) {
      return new ErrandHistoryDto {
        Description = errand.Description,
        Report = errand.Report,
        DueDate = errand.DueDate.ToString("d"),
        CompleteDate = errand.CompleteDate.Value.ToString("d"),
        EmployeeName = errand.Employee.GetFullName(),
        FacilityName = errand.Facility.CompanyName,
        Points = errand.Points.Select(p => new PointReviewHistoryDto {
          Order = p.Point.Order,
          TrapName = p.Point.Trap.TrapName,
          SupplementName = p.Point.Supplement.SupplementName,
          PerimeterName = p.Point.Perimeter.PerimeterName,
          Status = p.Status.ToString()
        }).ToList()
      };
    }
  }

  public class PointReviewHistoryDto {
    public int Order { get; init; }
    public string TrapName { get; init; }
    public string SupplementName { get; init; }
    public string PerimeterName { get; init; }
    public string Status { get; init; }
  }
}
