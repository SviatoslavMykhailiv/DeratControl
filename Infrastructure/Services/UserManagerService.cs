using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Services {
  public class UserManagerService : IUserManagerService {
    private readonly UserManager<ApplicationUser> userManager;
    private readonly IDeratControlDbContext context;

    public UserManagerService(UserManager<ApplicationUser> userManager, IDeratControlDbContext context) {
      this.userManager = userManager;
      this.context = context;
    }

    public async Task<Guid> SaveUser(
      string username,
      string password,
      string firstName,
      string lastName,
      string phoneNumber,
      Guid? facilityId) {

      var user = await userManager.FindByNameAsync(username);

      Facility facility = null;

      if (facilityId.HasValue)
        facility = await context.Facilities.FindAsync(facilityId.Value);

      if (user is null) {
        user = new ApplicationUser { UserName = username, FirstName = firstName, LastName = lastName, PhoneNumber = phoneNumber, Facility = facility };
        await userManager.CreateAsync(user, password);
      }
      else {
        user.FirstName = firstName;
        user.LastName = lastName;
        user.PhoneNumber = phoneNumber;
        user.Facility = facility;

        await userManager.UpdateAsync(user);
      }

      return user.Id;
    }

    public async Task DeleteUser(Guid userId) {

    }

    public async Task<IUser> GetUser(Guid userId) {
      var user = await userManager.FindByIdAsync(userId.ToString());

      if (user is null)
        return null;

      return user;
    }
  }
}
