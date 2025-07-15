using Application.Common.Interfaces;
using Application.Common.Models;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services
{
    public class CurrentUserProvider(IHttpContextAccessor httpContextAccessor) : ICurrentUserProvider
    {
        private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;

        public CurrentUser User => httpContextAccessor.HttpContext?.Items["CurrentUser"] as CurrentUser;
    }
}
