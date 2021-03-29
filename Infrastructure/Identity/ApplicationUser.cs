using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Infrastructure.Identity {
  public class ApplicationUser : IdentityUser<Guid>, IUser {
    public Guid UserId => Id;
    public Guid? FacilityId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Location { get; set; }
    public Facility Facility { get; set; }
    public ICollection<Errand> Errands { get; set; }
    public ICollection<DefaultFacility> DefaultFacilities { get; set; }
  }
}
