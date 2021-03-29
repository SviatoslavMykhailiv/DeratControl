using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Traps.Queries.GetTrapList {
  public class GetTrapListQuery : IRequest<TrapListVm> {
    public class GetTrapListQueryHandler : IRequestHandler<GetTrapListQuery, TrapListVm> {
      private readonly IDeratControlDbContext context;
      private readonly IMapper mapper;
      private readonly IMemoryCache cache;

      public GetTrapListQueryHandler(IDeratControlDbContext context, IMapper mapper, IMemoryCache cache) {
        this.context = context;
        this.mapper = mapper;
        this.cache = cache;
      }

      public async Task<TrapListVm> Handle(GetTrapListQuery request, CancellationToken cancellationToken) {
        var traps = await cache.GetOrCreateAsync(nameof(Trap), entry => {
          return context
          .Traps
          .ProjectTo<TrapDto>(mapper.ConfigurationProvider)
          .OrderBy(c => c.TrapName)
          .ToListAsync(cancellationToken);
        });

        return new TrapListVm { Traps = traps };
      }
    }
  }
}
