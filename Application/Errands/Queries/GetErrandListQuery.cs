using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Errands.Queries {
  public class GetErrandListQuery : IRequest<IEnumerable<ErrandDto>> {
    public class GetErrandListQueryHandler : BaseRequestHandler<GetErrandListQuery, IEnumerable<ErrandDto>> {
      private readonly IDeratControlDbContext db;
      private readonly IMediator mediator;

      public GetErrandListQueryHandler(
        ICurrentUserProvider currentUserProvider,
        ICurrentDateService currentDateService,
        IDeratControlDbContext db,
        IMediator mediator) : base(currentDateService, currentUserProvider) {
        this.db = db;
        this.mediator = mediator;
      }

      protected override async Task<IEnumerable<ErrandDto>> Handle(RequestContext context, GetErrandListQuery request, CancellationToken cancellationToken) {
        if (context.CurrentUser.Role == UserRole.Employee)
          return await mediator.Send(new GetEmployeeErrandListQuery(), cancellationToken);
        var errands = await GetErrandsQuery().ToListAsync(cancellationToken: cancellationToken);        
        return errands.Select(e => ErrandDto.Map(e, context.CurrentUser));
      }

      private IQueryable<Errand> GetErrandsQuery() {
        return db
          .Errands
          .Include(e => e.Employee)
          .Include(e => e.Facility)
          .ThenInclude(f => f.Perimeters)
          .ThenInclude(p => p.Points)
          .ThenInclude(p => p.Reviews)
          .ThenInclude(p => p.Records)
          .ThenInclude(r => r.Field)
          .ThenInclude(f => f.Trap)
          .OrderByDescending(e => e.DueDate)
          .AsNoTracking();
      }
    }
  }
}
