using System;
using System.Collections.Generic;

namespace Application.Common.Dtos {
  public class UserDto {
    public Guid? UserId { get; init; }
    public string UserName { get; init; }
    public string Password { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string PhoneNumber { get; init; }
    public Guid? FacilityId { get; init; }
    public string Location { get; init; }
    public string Role { get; init; }
    public IReadOnlyCollection<Guid> Facilities { get; init; } = new List<Guid>();
  }
}
