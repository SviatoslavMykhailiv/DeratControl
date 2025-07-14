using Application.Common.Dtos;
using Application.Common.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class DataSeeder
    {
        private readonly DeratControlDbContext context;
        private readonly IUserManagerService userManagerService;
        private readonly RoleManager<ApplicationRole> roleManager;

        public DataSeeder(DeratControlDbContext context, IUserManagerService userManagerService, RoleManager<ApplicationRole> roleManager)
        {
            this.context = context;
            this.userManagerService = userManagerService;
            this.roleManager = roleManager;
        }

        public async Task Seed()
        {
            if (!await context.Roles.AnyAsync())
            {
                await roleManager.CreateAsync(new ApplicationRole { Name = "PROVIDER", NormalizedName = "PROVIDER", ConcurrencyStamp = Guid.NewGuid().ToString() });
                await roleManager.CreateAsync(new ApplicationRole { Name = "CUSTOMER", NormalizedName = "CUSTOMER", ConcurrencyStamp = Guid.NewGuid().ToString() });
                await roleManager.CreateAsync(new ApplicationRole { Name = "EMPLOYEE", NormalizedName = "EMPLOYEE", ConcurrencyStamp = Guid.NewGuid().ToString() });
            }

            if (!await context.Users.AnyAsync())
            {
                await userManagerService.SaveUser(null, new UserDto
                {
                    UserName = "admin",
                    Password = "V@lr1n0k",
                    FirstName = "admin",
                    LastName = "admin",
                    PhoneNumber = "+380638660938",
                    Role = "PROVIDER"
                });
            }
        }
    }
}
