using Application.Common.Dtos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IUserManagerService
    {
        Task<Guid> SaveUser(Guid? providerId, UserDto user, CancellationToken cancellationToken = default);
        Task DeleteUser(Guid userId, CancellationToken cancellationToken = default);
        Task<IUser> GetUser(Guid userId, CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<IUser>> GetEmployeeList(Guid providerId, bool includeInactive = true, CancellationToken cancellationToken = default);
        Task SetUserAvailability(Guid userId, bool available);
        Task UpdateProviderName(Guid userId, string providerName);
    }
}
