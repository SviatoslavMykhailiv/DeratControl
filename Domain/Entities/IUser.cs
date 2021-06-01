using Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public interface IUser
    {
        Guid UserId { get; }
        Guid? FacilityId { get; set; }
        string UserName { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string PhoneNumber { get; set; }
        string Location { get; set; }
        string ProviderName { get; set; }
        Facility Facility { get; set; }
        Guid? ProviderId { get; set; }
        IUser Provider { get; set; }
        ICollection<Errand> Errands { get; set; }
        ICollection<DefaultFacility> DefaultFacilities { get; set; }
        bool Available { get; set; }
        string GetFullName();
        bool HasDefaultFacility(Guid facilityId);
        Device Device { get; set; }
    }
}
