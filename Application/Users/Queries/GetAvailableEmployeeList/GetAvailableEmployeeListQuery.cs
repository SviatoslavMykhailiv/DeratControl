using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries.GetAvailableEmployeeList
{
    internal class UserDefaultFacilityComparer : IComparer<IUser>
    {
        private readonly Guid facilityId;

        public UserDefaultFacilityComparer(Guid facilityId)
        {
            this.facilityId = facilityId;
        }

        public int Compare([AllowNull] IUser x, [AllowNull] IUser y)
        {
            if (x.HasDefaultFacility(facilityId) && y.HasDefaultFacility(facilityId))
                return 0;

            if (x.HasDefaultFacility(facilityId))
                return -1;

            return 1;
        }
    }

    public record GetAvailableEmployeeListQuery : IRequest<IEnumerable<UserDto>>
    {
        public Guid? FacilityId { get; init; }
        public DateTime? DueDate { get; init; }
        public Guid? ErrandId { get; init; }

        public class GetAvailableEmployeeListQueryHandler : BaseRequestHandler<GetAvailableEmployeeListQuery, IEnumerable<UserDto>>
        {
            private readonly IUserManagerService userManagerService;
            private readonly IMapper mapper;
            private readonly IDeratControlDbContext db;

            public GetAvailableEmployeeListQueryHandler(
              ICurrentDateService currentDateService,
              ICurrentUserProvider currentUserProvider,
              IUserManagerService userManagerService,
              IMapper mapper,
              IDeratControlDbContext db) : base(currentDateService, currentUserProvider)
            {
                this.userManagerService = userManagerService;
                this.mapper = mapper;
                this.db = db;
            }

            protected override async Task<IEnumerable<UserDto>> Handle(RequestContext context, GetAvailableEmployeeListQuery request, CancellationToken cancellationToken)
            {
                var employeeList = (await userManagerService.GetEmployeeList(context.CurrentUser.UserId, false, cancellationToken)).ToList();
                var busyEmployees = new HashSet<Guid>();

                if (request.FacilityId.HasValue && request.DueDate.HasValue)
                {
                    busyEmployees = db
                      .Errands
                      .Where(e => e.FacilityId == request.FacilityId && e.DueDate.Date == request.DueDate.Value.Date && (request.ErrandId.HasValue == false || e.Id != request.ErrandId.Value))
                      .Select(e => e.EmployeeId)
                      .ToHashSet();

                    employeeList.Sort(new UserDefaultFacilityComparer(request.FacilityId.Value));
                }

                return employeeList
                  .Where(e => busyEmployees.Contains(e.UserId) == false)
                  .Select(user => mapper.Map<UserDto>(user));
            }
        }
    }
}
