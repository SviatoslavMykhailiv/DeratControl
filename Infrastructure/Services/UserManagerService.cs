using Application.Common.Dtos;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserManagerService : IUserManagerService
    {

        private const string CUSTOMER = nameof(CUSTOMER);
        private const string EMPLOYEE = nameof(EMPLOYEE);
        private const string PROVIDER = nameof(PROVIDER);

        private readonly UserManager<ApplicationUser> userManager;
        private readonly DeratControlDbContext context;

        public UserManagerService(
          UserManager<ApplicationUser> userManager,
          DeratControlDbContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }

        public async Task<Guid> SaveUser(Guid? providerId, UserDto userModel, CancellationToken cancellationToken = default)
        {
            ApplicationUser user = null;
            Facility facility = null;

            if (userModel.FacilityId.HasValue)
                facility = await context.Facilities.FindAsync(new object[] { userModel.FacilityId.Value }, cancellationToken: cancellationToken);

            var defaultFacilities = await context.Facilities.Where(f => userModel.Facilities.Contains(f.Id)).ToListAsync(cancellationToken: cancellationToken);

            if (userModel.UserId is null)
            {

                var provider = providerId.HasValue ? await GetUserInternal(providerId.Value, cancellationToken) : null;

                user = new ApplicationUser
                {
                    Provider = provider,
                    UserName = userModel.UserName,
                    FirstName = userModel.FirstName,
                    LastName = userModel.LastName,
                    PhoneNumber = userModel.PhoneNumber,
                    Facility = facility,
                    Location = userModel.Location,
                    Available = true,
                    Device = userModel.Device,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    DefaultFacilities = defaultFacilities.Select(f => new DefaultFacility { Facility = f }).ToList()
                };

                var result = await userManager.CreateAsync(user, userModel.Password);

                if (result.Succeeded == false)
                    throw new BadRequestException("Не вдалось створити користувача.");

                await userManager.AddToRoleAsync(user, userModel.Role);
            }
            else
            {
                user = await GetUserInternal(userModel.UserId.Value, cancellationToken);

                user.FirstName = userModel.FirstName;
                user.LastName = userModel.LastName;
                user.PhoneNumber = userModel.PhoneNumber;
                user.Facility = facility;
                user.Location = userModel.Location;
                user.Device = userModel.Device;
                user.DefaultFacilities = defaultFacilities.Select(f => new DefaultFacility { Facility = f }).ToList();

                await userManager.UpdateAsync(user);
            }

            return user.Id;
        }

        public async Task DeleteUser(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());

            if (user is null)
                return;

            await userManager.DeleteAsync(user);
        }

        public async Task<IUser> GetUser(Guid userId, CancellationToken cancellationToken = default)
        {
            return await GetUserInternal(userId, cancellationToken);
        }

        public async Task<IReadOnlyCollection<IUser>> GetEmployeeList(Guid providerId, bool includeInactive = true, CancellationToken cancellationToken = default)
        {
            return await (from user in context.Users.Include(u => u.DefaultFacilities)
                          join userRole in context.UserRoles
                          on user.Id equals userRole.UserId
                          join role in context.Roles
                          on userRole.RoleId equals role.Id
                          where role.Name == EMPLOYEE && (includeInactive || user.Available) && user.ProviderId == providerId
                          select user).AsNoTracking()
                    .ToListAsync(cancellationToken);
        }

        public async Task SetUserAvailability(Guid userId, bool available)
        {
            var user = await userManager.FindByIdAsync(userId.ToString()) ?? throw new NotFoundException("Користувач не знайдений.");
            user.Available = available;
            await userManager.UpdateAsync(user);
        }

        public async Task<ApplicationUser> GetUserInternal(Guid userId, CancellationToken cancellationToken = default)
        {
            return await context
              .Users
              .Include(u => u.DefaultFacilities)
              .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken: cancellationToken);
        }
    }
}
