using Domain.Common;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities {
  public class Facility : AuditableEntity {

    private readonly HashSet<Perimeter> perimeters = new HashSet<Perimeter>();
    private readonly HashSet<IUser> users = new HashSet<IUser>();

    public string CompanyName { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public string Address { get; set; }
    public string SecurityCode { get; set; }

    public IEnumerable<Perimeter> Perimeters => perimeters;
    public IEnumerable<IUser> Users => users;

    public void RemovePoint(Guid pointId) {
      var point = Perimeters.SelectMany(p => p.Points).FirstOrDefault(p => p.Id == pointId);

      if (point is null)
        return;

      point.Perimeter.RemovePoint(pointId);
    }

    public IEnumerable<QRID> GenerateQRList(int count, IReadOnlyCollection<Guid> perimeterList, IReadOnlyCollection<Trap> trapList) {
      foreach (var perimeter in perimeters.Where(c => perimeterList.Contains(c.Id)))
        foreach (var trap in trapList)
          foreach (var order in Enumerable.Range(1, count))
            yield return new QRID(perimeter.Id, order, trap.Id);
    }

    public Perimeter GetPerimeter(Guid perimeterId) => perimeters.FirstOrDefault(p => p.Id == perimeterId);
  }
}
