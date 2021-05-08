using Application.Common.Interfaces;
using Application.Common.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace Infrastructure.Services {
  public class CurrentUserProvider : ICurrentUserProvider {
    private const string USER_ID_CLAIM = "derat-user-id";
    private const string USER_ROLE_CLAIM = "derat-user-role";
    private const string FACILITY_ID_CLAIM = "derat-facility-id";

    public CurrentUserProvider(IHttpContextAccessor httpContextAccessor) {

      if (httpContextAccessor.HttpContext is null)
        return;

      if (httpContextAccessor.HttpContext.User is null)
        return;

      var userIdClaim = httpContextAccessor.HttpContext.User.FindFirstValue(USER_ID_CLAIM);

      if (userIdClaim is null)
        return;

      var userRoleClaim = httpContextAccessor.HttpContext.User.FindFirstValue(USER_ROLE_CLAIM);
      var facilityIdClaim = httpContextAccessor.HttpContext.User.FindFirstValue(FACILITY_ID_CLAIM);

      Guid? facilityId = string.IsNullOrEmpty(facilityIdClaim) ? 
        null : 
        Guid.Parse(facilityIdClaim);

      User = new CurrentUser(Guid.Parse(userIdClaim), Enum.Parse<UserRole>(userRoleClaim, true), facilityId);
    }

    public CurrentUser User { get; }
  }
}
