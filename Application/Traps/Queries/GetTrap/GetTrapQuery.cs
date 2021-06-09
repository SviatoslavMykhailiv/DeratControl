using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Traps.Queries.GetTrap
{
    public record GetTrapQuery : IRequest<TrapDto>
    {
        public GetTrapQuery(Guid trapId)
        {
            TrapId = trapId;
        }

        public Guid TrapId { get; init; }

        public class GetTrapQueryHandler : BaseRequestHandler<GetTrapQuery, TrapDto>
        {
            private readonly IMemoryCache cache;
            private readonly IDeratControlDbContext db;
            private readonly IMapper mapper;

            public GetTrapQueryHandler(
                ICurrentDateService currentDateService,
                ICurrentUserProvider currentUserProvider,
                IMemoryCache cache,
                IDeratControlDbContext db,
                IMapper mapper) : base(currentDateService, currentUserProvider)
            {
                this.cache = cache;
                this.db = db;
                this.mapper = mapper;
            }

            protected override async Task<TrapDto> Handle(RequestContext context, GetTrapQuery request, CancellationToken cancellationToken)
            {
                var cached = cache.Get<List<TrapDto>>($"{nameof(Trap)}-{context.CurrentUser.UserId}");

                if (cached is null)
                {
                    return await db
                    .Traps
                    .Include(t => t.Fields)
                    .ProjectTo<TrapDto>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(t => t.TrapId == request.TrapId, cancellationToken: cancellationToken) ?? throw new NotFoundException("Пастку не знайдено.");
                }

                return cached.Find(c => c.TrapId == request.TrapId);
            }
        }
    }
}