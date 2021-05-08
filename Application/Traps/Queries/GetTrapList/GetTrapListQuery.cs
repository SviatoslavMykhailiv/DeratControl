using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Traps.Queries.GetTrapList {
  public record GetTrapListQuery : IRequest<IEnumerable<TrapDto>> {
    public class GetTrapListQueryHandler : IRequestHandler<GetTrapListQuery, IEnumerable<TrapDto>> {
      private readonly IDeratControlDbContext context;
      private readonly IMemoryCache cache;
      private readonly IMapper mapper;

      public GetTrapListQueryHandler(IDeratControlDbContext context, IMemoryCache cache, IMapper mapper) {
        this.context = context;
        this.cache = cache;
        this.mapper = mapper;
      }

      public async Task<IEnumerable<TrapDto>> Handle(GetTrapListQuery request, CancellationToken cancellationToken) {
        var traps = await cache.GetOrCreateAsync(nameof(Trap), entry => {
          return context
          .Traps
          .Include(t => t.Fields)
          .AsNoTracking()
          .OrderBy(c => c.TrapName)
          .ProjectTo<TrapDto>(mapper.ConfigurationProvider)
          .ToListAsync(cancellationToken);
        });

        return traps;
      }
    }
  }
}
