﻿using System;
using System.Collections.Generic;

namespace Domain.Entities {
  public interface IUser {
    Guid UserId { get; }
    Guid? FacilityId { get; set; }
    string FirstName { get; set; }
    string LastName { get; set; }
    string PhoneNumber { get; set; }
    string Location { get; set; }
    Facility Facility { get; set; }
    ICollection<Errand> Errands { get; set; }
    ICollection<DefaultFacility> DefaultFacilities { get; set; }
  }
}