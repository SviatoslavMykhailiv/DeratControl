using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities {
  public class Perimeter : AuditableEntity {
    public Perimeter() {
      Points = new HashSet<Point>();
    }

    public Guid PerimeterId { get; set; }
    public Guid FacilityId { get; set; }

    public int LeftLoc { get; set; }
    public int TopLoc { get; set; }
    public string PerimeterName { get; set; }
    public string SchemeImagePath { get; set; }
    public Facility Facility { get; set; }
    public ICollection<Point> Points { get; private set; }

    public void UpdatePoints(IReadOnlyCollection<Point> points) {
      var existingPoints = Points.ToDictionary(p => p.PointId);

      foreach (var point in Points.ToList()) {
        if (points.Any(p => p.PointId == point.PointId))
          continue;

        Points.Remove(point);
      }

      foreach (var inputPoint in points) {
        if (existingPoints.ContainsKey(inputPoint.PointId)) {
          var point = existingPoints[inputPoint.PointId];

          point.TopLoc = inputPoint.TopLoc;
          point.LeftLoc = inputPoint.LeftLoc;
          point.Order = inputPoint.Order;
          point.Supplement = inputPoint.Supplement;
          point.Trap = inputPoint.Trap;
        }
        else {
          Points.Add(inputPoint);
        }
      }
    }
  }
}
