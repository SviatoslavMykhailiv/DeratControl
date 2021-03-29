using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace API.Services {
  public class CurrentUserService : ICurrentUserService {
    private const string USER_ID_CLAIM = "derat-user-id";

    public CurrentUserService(IHttpContextAccessor httpContextAccessor) {
      var userId = httpContextAccessor.HttpContext?.User?.FindFirstValue(USER_ID_CLAIM);
      Guid.TryParse(userId, out var id);
      UserId = id;
    }

    public Guid UserId { get; }
  }
}
