using Domain.Common;
using System;

namespace Domain.Entities {
  public class DefaultFacility : AuditableEntity {
    public Guid FacilityId { get; set; }
    public Guid UserId { get; set; }

    public Facility Facility { get; set; }
    public IUser User { get; set; }
  }
}
