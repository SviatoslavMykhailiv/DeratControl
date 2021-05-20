using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.Enums;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Domain.Entities;

namespace Application.Errands.Queries {
  public record GetEmployeeErrandListQuery : IRequest<IEnumerable<ErrandDto>> {

    public class GetEmployeeErrandListQueryHandler : BaseRequestHandler<GetEmployeeErrandListQuery, IEnumerable<ErrandDto>> {
      private readonly IDeratControlDbContext db;

      public GetEmployeeErrandListQueryHandler(
        IDeratControlDbContext db,
        ICurrentDateService currentDateService,
        ICurrentUserProvider currentUserProvider) : base(currentDateService, currentUserProvider) {
        this.db = db;
      }

      protected override async Task<IEnumerable<ErrandDto>> Handle(RequestContext context, GetEmployeeErrandListQuery request, CancellationToken cancellationToken) {
        var errands = await GetErrandsQuery(context.CurrentUser).ToListAsync(cancellationToken: cancellationToken);

        foreach (var errand in errands)
          errand.MoveDueDate(context.CurrentDateTime);

        await db.SaveChangesAsync(cancellationToken);

        return errands.Select(e => ErrandDto.Map(e, context.CurrentUser));
      }

      private IQueryable<Errand> GetErrandsQuery(CurrentUser user) {
        return db
          .Errands
          .Include(e => e.Employee)
          .Include(e => e.Facility)
          .ThenInclude(f => f.Perimeters)
          .ThenInclude(p => p.Points)
          .ThenInclude(p => p.Trap)
          .Include(e => e.Points)
          .ThenInclude(e => e.Records)
          .ThenInclude(r => r.Field)
          .Where(e => e.EmployeeId == user.UserId && e.Status == ErrandStatus.Planned)
          .OrderByDescending(e => e.DueDate);
      }
    }

  }
}
