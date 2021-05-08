using Application.Common.Dtos;
using Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces {
  public interface IUserManagerService {
    Task<Guid> SaveUser(UserDto user, CancellationToken cancellationToken = default);
    Task DeleteUser(Guid userId, CancellationToken cancellationToken = default);
    Task<IUser> GetUser(Guid userId, CancellationToken cancellationToken = default);
  }
}
