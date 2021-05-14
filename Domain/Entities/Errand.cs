using Domain.Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities {
  /// <summary>
  /// Represents an errand.
  /// </summary>
  public class Errand : AuditableEntity {

    private DateTime? completeDate;
    private DateTime dueDate;
    private DateTime originalDueDate;
    private ErrandStatus status;
    private string report;
    private string description;

    private readonly HashSet<PointReview> points = new HashSet<PointReview>();

    /// <summary>
    /// Initializes a new instance of <see cref="Errand"/> with Planned status.
    /// </summary>
    public Errand() {
      Status = ErrandStatus.Planned;
    }

    public Guid FacilityId { get; init; }
    public Guid EmployeeId { get; init; }

    public DateTime OriginalDueDate {
      get => originalDueDate;
      private set {
        AssertErrandNotFinished();
        originalDueDate = value;
      }
    }

    public DateTime DueDate {
      get => dueDate;
      private set {
        AssertErrandNotFinished();
        dueDate = value;
      }
    }

    public DateTime? CompleteDate {
      get => completeDate;
      private set {
        AssertErrandNotFinished();
        completeDate = value;
      }
    }

    public ErrandStatus Status {
      get => status;
      private set {
        AssertErrandNotFinished();
        status = value;
      }
    }

    public string Description {
      get => description;
      set {
        if (string.IsNullOrWhiteSpace(value))
          throw new InvalidOperationException("Description cannot be empty.");

        description = value;
      }
    }
    
    public string Report {
      get => report;
      set {
        if (string.IsNullOrWhiteSpace(value))
          throw new InvalidOperationException("Report cannot be empty.");

        report = value;
      }
    }

    public Facility Facility { get; set; }
    public IUser Employee { get; set; }

    public IEnumerable<PointReview> Points => points;

    public bool IsOverdue(DateTime currentDate) => DueDate < currentDate;

    public void SetDueDate(DateTime dueDate) {
      AssertErrandNotFinished();

      DueDate = dueDate;
      OriginalDueDate = dueDate;
    }

    public void MoveDueDate(DateTime currentDate) {

      AssertErrandNotFinished();

      DueDate = currentDate.AddDays(1);
      Status = ErrandStatus.Overdue;
    }

    public void Complete(DateTime completeDate, string report) {
      AssertErrandNotFinished();

      CompleteDate = completeDate;
      Report = report;
      Status = ErrandStatus.Finished;
    }

    public void SetPointListForReview(IEnumerable<Guid> selectedPointList) {
      AssertErrandNotFinished();

      var inputPointSet = selectedPointList.ToHashSet();

      var existingPointList = points.ToDictionary(p => p.PointId);

      var removePointList = from pointId in existingPointList.Keys
                             where inputPointSet.Contains(pointId) == false
                             select pointId;

      points.RemoveWhere(p => removePointList.Contains(p.PointId));

      var pointList = Facility.Perimeters.SelectMany(p => p.Points).ToDictionary(p => p.Id);

      foreach(var pointId in selectedPointList.Where(id => existingPointList.ContainsKey(id) == false)) {
        var pointReview = new PointReview(pointList[pointId]);
        points.Add(pointReview);
      }
    }

    private void AssertErrandNotFinished() {
      if (status == ErrandStatus.Finished)
        throw new InvalidOperationException("Errand is finished.");
    }

    public bool IsSecurityCodeValid(string securityCode) => string.Equals(securityCode, Facility.SecurityCode, StringComparison.OrdinalIgnoreCase);
  }
}
