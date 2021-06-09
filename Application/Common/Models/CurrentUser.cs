using Application.Common.Interfaces;
using System;

namespace Application.Common.Models
{
    public record CurrentUser(Guid UserId, UserRole Role, Guid? FacilityId);
}
