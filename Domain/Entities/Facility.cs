using Domain.Common;
using System;
using System.Collections.Generic;

namespace Domain.Entities {
  public class Facility : AuditableEntity {
    public Facility() {
      Perimeters = new HashSet<Perimeter>();
      Users = new HashSet<IUser>();
    }

    public Guid FacilityId { get; set; }
    public string CompanyName { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string SecurityCode { get; set; }

    public ICollection<Perimeter> Perimeters { get; private set; }
    public ICollection<IUser> Users { get; private set; }
  }
}
