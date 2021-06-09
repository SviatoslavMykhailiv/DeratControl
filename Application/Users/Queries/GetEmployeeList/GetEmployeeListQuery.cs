using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries.GetEmployeeList
{
    public record GetEmployeeListQuery : IRequest<IEnumerable<UserDto>>
    {
        public class GetEmployeeListQueryHandler : BaseRequestHandler<GetEmployeeListQuery, IEnumerable<UserDto>>
        {
            private readonly IUserManagerService userManager;
            private readonly IMapper mapper;

            public GetEmployeeListQueryHandler(ICurrentDateService currentDateService, ICurrentUserProvider currentUserProvider, IUserManagerService userManager, IMapper mapper) : base(currentDateService, currentUserProvider)
            {
                this.userManager = userManager;
                this.mapper = mapper;
            }

            protected override async Task<IEnumerable<UserDto>> Handle(RequestContext context, GetEmployeeListQuery request, CancellationToken cancellationToken)
            {
                var result = (await userManager.GetEmployeeList(context.CurrentUser.UserId, true, cancellationToken)).ToList();
                return result.Select(r => mapper.Map<UserDto>(r));
            }
        }
    }
}
