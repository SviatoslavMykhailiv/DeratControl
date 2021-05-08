using Domain.Common;
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
  }
}
