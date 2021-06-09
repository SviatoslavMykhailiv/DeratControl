using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Traps.Queries.GetTrapList
{
    public record GetTrapListQuery : IRequest<IEnumerable<TrapDto>>
    {
        public class GetTrapListQueryHandler : BaseRequestHandler<GetTrapListQuery, IEnumerable<TrapDto>>
        {
            private readonly IDeratControlDbContext db;
            private readonly IMemoryCache cache;
            private readonly IMapper mapper;

            public GetTrapListQueryHandler(
              ICurrentDateService currentDateService,
              ICurrentUserProvider currentUserProvider,
              IDeratControlDbContext db,
              IMemoryCache cache,
              IMapper mapper) : base(currentDateService, currentUserProvider)
            {
                this.db = db;
                this.cache = cache;
                this.mapper = mapper;
            }

            protected override async Task<IEnumerable<TrapDto>> Handle(RequestContext context, GetTrapListQuery request, CancellationToken cancellationToken)
            {
                var traps = await cache.GetOrCreateAsync($"{nameof(Trap)}-{context.CurrentUser.UserId}", async entry =>
                {
                    var result = await db
                    .Traps.Where(t => t.ProviderId == context.CurrentUser.UserId)
                    .Include(t => t.Fields)
                    .AsNoTracking()
                    .OrderBy(c => c.TrapName)
                    .ToListAsync(cancellationToken);

                    return result.Select(r => mapper.Map<TrapDto>(r)).ToList();
                });

                return traps;
            }
        }
    }
}
