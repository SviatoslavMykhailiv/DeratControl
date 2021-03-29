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

namespace Application.Supplements.Queries {
  public class GetSupplementListQuery : IRequest<SupplementListVm> {
    public class GetSupplementListQueryHandler : IRequestHandler<GetSupplementListQuery, SupplementListVm> {
      private readonly IDeratControlDbContext context;
      private readonly IMapper mapper;
      private readonly IMemoryCache cache;

      public GetSupplementListQueryHandler(IDeratControlDbContext context, IMapper mapper, IMemoryCache cache) {
        this.context = context;
        this.mapper = mapper;
        this.cache = cache;
      }
      
      public async Task<SupplementListVm> Handle(GetSupplementListQuery request, CancellationToken cancellationToken) {
        var supplements = await cache.GetOrCreateAsync(nameof(Supplement), entry => {
          return context
          .Supplements
          .ProjectTo<SupplementDto>(mapper.ConfigurationProvider)
          .OrderBy(c => c.SupplementName)
          .ToListAsync(cancellationToken);
        });

        return new SupplementListVm { Supplements = supplements };
      }
    }
  }
}
