using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries.GetEmployeeList {
  internal class UserDefaultFacilityComparer : IComparer<IUser> {
    private readonly Guid facilityId;

    public UserDefaultFacilityComparer(Guid facilityId) {
      this.facilityId = facilityId;
    }

    public int Compare([AllowNull] IUser x, [AllowNull] IUser y) {
      if (x.HasDefaultFacility(facilityId) && y.HasDefaultFacility(facilityId))
        return 0;

      if (x.HasDefaultFacility(facilityId))
        return -1;

      return 1;
    }
  }

  public record GetEmployeeListQuery : IRequest<IEnumerable<UserDto>> {

    public Guid? FacilityId { get; init; }

    public class GetEmployeeListQueryHandler : IRequestHandler<GetEmployeeListQuery, IEnumerable<UserDto>> {
      private readonly IUserManagerService userManager;
      private readonly IMapper mapper;

      public GetEmployeeListQueryHandler(IUserManagerService userManager, IMapper mapper) {
        this.userManager = userManager;
        this.mapper = mapper;
      }

      public async Task<IEnumerable<UserDto>> Handle(GetEmployeeListQuery request, CancellationToken cancellationToken) {
        var result = (await userManager.GetEmployeeList(cancellationToken)).ToList();

        if (request.FacilityId.HasValue)
          result.Sort(new UserDefaultFacilityComparer(request.FacilityId.Value));

        return result.Select(r => mapper.Map<UserDto>(r));
      }
    }
  }
}
