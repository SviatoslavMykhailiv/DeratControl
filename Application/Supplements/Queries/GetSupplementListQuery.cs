using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Models;
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

namespace Application.Supplements.Queries {
  public class GetSupplementListQuery : IRequest<IEnumerable<SupplementDto>> {
    public class GetSupplementListQueryHandler : BaseRequestHandler<GetSupplementListQuery, IEnumerable<SupplementDto>> {
      private readonly IDeratControlDbContext db;
      private readonly IMapper mapper;
      private readonly IMemoryCache cache;
      private readonly IFileStorage fileStorage;

      public GetSupplementListQueryHandler(
        ICurrentDateService currentDateService, 
        ICurrentUserProvider currentUserProvider,
        IDeratControlDbContext db, 
        IMapper mapper, 
        IMemoryCache cache, 
        IFileStorage fileStorage) : base(currentDateService, currentUserProvider) {
        this.db = db;
        this.mapper = mapper;
        this.cache = cache;
        this.fileStorage = fileStorage;
      }

      protected override async Task<IEnumerable<SupplementDto>> Handle(RequestContext context, GetSupplementListQuery request, CancellationToken cancellationToken) {
        var supplements = await cache.GetOrCreateAsync($"{nameof(Supplement)}-{context.CurrentUser.UserId}", entry => {
          return db
          .Supplements
          .Where(s => s.ProviderId == context.CurrentUser.UserId)
          .AsNoTracking()
          .ProjectTo<SupplementDto>(mapper.ConfigurationProvider)
          .OrderBy(c => c.SupplementName)
          .ToListAsync(cancellationToken);
        });

        return supplements;
      }
    }
  }
}
