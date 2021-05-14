using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Errands.Queries.GetErrand {
  public record GetErrandQuery : IRequest<ErrandDto> {
    public GetErrandQuery(Guid errandId) {
      ErrandId = errandId;
    }

    public Guid ErrandId { get; }

    public class GetErrandQueryHandler : IRequestHandler<GetErrandQuery, ErrandDto> {
      private readonly IDeratControlDbContext db;
      private readonly IMapper mapper;

      public GetErrandQueryHandler(IDeratControlDbContext db, IMapper mapper) {
        this.db = db;
        this.mapper = mapper;
      }

      public async Task<ErrandDto> Handle(GetErrandQuery request, CancellationToken cancellationToken) {
        var errand = await db.Errands
                 .AsNoTracking()
                 .Where(e => e.Id == request.ErrandId)
                 .ProjectTo<ErrandDto>(mapper.ConfigurationProvider)
                 .FirstOrDefaultAsync(cancellationToken);

        if (errand is null)
          throw new NotFoundException();

        return errand;
      }
    }
  }
}
