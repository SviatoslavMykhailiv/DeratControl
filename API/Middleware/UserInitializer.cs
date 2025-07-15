using Application.Common.Interfaces;
using Application.Common.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Middleware;

public class UserInitializer : IMiddleware
{
    private const string USER_ID_CLAIM = "derat-user-id";
    private const string USER_ROLE_CLAIM = "derat-user-role";
    private const string FACILITY_ID_CLAIM = "derat-facility-id";

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.User is null)
        {
            await next(context);

            return;
        }

        var userIdClaim = context.User.FindFirstValue(USER_ID_CLAIM);

        if (userIdClaim is null)
        {
            await next(context);

            return;
        }

        var userRoleClaim = context.User.FindFirstValue(USER_ROLE_CLAIM);
        var facilityIdClaim = context.User.FindFirstValue(FACILITY_ID_CLAIM);

        Guid? facilityId = string.IsNullOrEmpty(facilityIdClaim) ?
          null :
          Guid.Parse(facilityIdClaim);

        context.Items["CurrentUser"] = new CurrentUser(Guid.Parse(userIdClaim), Enum.Parse<UserRole>(userRoleClaim, true), facilityId);

        await next(context);
    }
}
