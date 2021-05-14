using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries.GetEmployeeList {
  public record GetEmployeeListQuery : IRequest<IEnumerable<UserDto>> {
    public class GetEmployeeListQueryHandler : IRequestHandler<GetEmployeeListQuery, IEnumerable<UserDto>> {
      private readonly IUserManagerService userManager;
      private readonly IMapper mapper;

      public GetEmployeeListQueryHandler(IUserManagerService userManager, IMapper mapper) {
        this.userManager = userManager;
        this.mapper = mapper;
      }

      public async Task<IEnumerable<UserDto>> Handle(GetEmployeeListQuery request, CancellationToken cancellationToken) {
        var result = (await userManager.GetEmployeeList(true, cancellationToken)).ToList();
        return result.Select(r => mapper.Map<UserDto>(r));
      }
    }
  }
}
