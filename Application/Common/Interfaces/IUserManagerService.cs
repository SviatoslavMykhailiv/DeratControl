using Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Application.Common.Interfaces {
  public interface IUserManagerService {
    Task<Guid> SaveUser(
      string username, 
      string password, 
      string firstName, 
      string lastName, 
      string phoneNumber,
      Guid? facilityId);
    Task DeleteUser(Guid userId);
    Task<IUser> GetUser(Guid userId);
  }
}
