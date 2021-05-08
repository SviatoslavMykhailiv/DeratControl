using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities {
  public class Perimeter : AuditableEntity {

    private readonly HashSet<Point> points = new HashSet<Point>();

    public Guid FacilityId { get; init; }

    public int LeftLoc { get; set; }
    public int TopLoc { get; set; }
    public string PerimeterName { get; set; }
    public Facility Facility { get; init; }
    public IEnumerable<Point> Points => points;

    public void SetPoint(Guid? pointId, int order, int leftLoc, int topLoc, Trap trap, Supplement supplement) {
      if (pointId.HasValue)
        UpdatePoint(pointId.Value, order, leftLoc, topLoc, trap, supplement);
      else
        AddPoint(order, leftLoc, topLoc, trap, supplement);
    }

    public void RemovePoint(Guid pointId) {
      points.RemoveWhere(p => p.Id == pointId);
    }

    private void AddPoint(int order, int leftLoc, int topLoc, Trap trap, Supplement supplement) {
      if (points.Any(p => p.Order == order && p.Trap == trap))
        throw new InvalidOperationException($"Point with order {order} already exists.");

      var point = new Point {
        Perimeter = this,
        Order = order,
        LeftLoc = leftLoc,
        TopLoc = topLoc,
        Trap = trap,
        Supplement = supplement
      };

      points.Add(point);
    }

    private void UpdatePoint(Guid pointId, int order, int leftLoc, int topLoc, Trap trap, Supplement supplement) {
      if (points.Any(p => p.Id != pointId && p.Order == order && p.Trap == trap))
        throw new InvalidOperationException($"Point with order {order} already exists.");

      var point = points.First(p => p.Id == pointId);

      if (point is null)
        return;

      point.Order = order;
      point.LeftLoc = leftLoc;
      point.TopLoc = topLoc;
      point.Trap = trap;
      point.Supplement = supplement;
    }
  }
}
