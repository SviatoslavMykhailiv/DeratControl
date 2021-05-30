using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Users.Queries.GetUser
{
    public record GetUserQuery : IRequest<UserDto>
    {
        public GetUserQuery(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; init; }
        public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
        {
            private readonly IUserManagerService userManagerService;
            private readonly IMapper mapper;

            public GetUserQueryHandler(IUserManagerService userManagerService, IMapper mapper)
            {
                this.userManagerService = userManagerService;
                this.mapper = mapper;
            }

            public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
            {
                var user = await userManagerService.GetUser(request.UserId, cancellationToken) ?? throw new NotFoundException();
                return mapper.Map<UserDto>(user);
            }
        }
    }
}