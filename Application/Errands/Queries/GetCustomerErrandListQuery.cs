using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.Enums;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Application.Errands.Queries {
  public record GetCustomerErrandListQuery : IRequest<IEnumerable<ErrandDto>> {
    public class GetCustomerErrandListQueryHandler : BaseRequestHandler<GetCustomerErrandListQuery, IEnumerable<ErrandDto>> {
      private readonly IDeratControlDbContext db;
      private readonly IMapper mapper;

      public GetCustomerErrandListQueryHandler(
        IDeratControlDbContext db,
        IMapper mapper,
        ICurrentDateService currentDateService,
        ICurrentUserProvider currentUserProvider) : base(currentDateService, currentUserProvider) {
        this.db = db;
        this.mapper = mapper;
      }

      protected override async Task<IEnumerable<ErrandDto>> Handle(RequestContext context, GetCustomerErrandListQuery request, CancellationToken cancellationToken) {
        var errands = await db.Errands
          .Where(e => e.FacilityId == context.CurrentUser.FacilityId.Value && e.Status == ErrandStatus.Finished)
          .AsNoTracking()
          .ProjectTo<ErrandDto>(mapper.ConfigurationProvider)
          .ToListAsync(cancellationToken: cancellationToken);

        return errands;
      }
    }
  }
}
