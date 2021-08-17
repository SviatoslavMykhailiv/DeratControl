using Application.Common;
using Application.Common.Dtos;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Errands.Queries
{
    public class GetErrandListQuery : IRequest<ItemList<ErrandDto>>
    {
        public int? Skip { get; init; } = 0;
        public int? Take { get; init; } = 10;

        public class GetErrandListQueryHandler : BaseRequestHandler<GetErrandListQuery, ItemList<ErrandDto>>
        {
            private readonly IDeratControlDbContext db;
            private readonly IMediator mediator;

            public GetErrandListQueryHandler(
              ICurrentUserProvider currentUserProvider,
              ICurrentDateService currentDateService,
              IDeratControlDbContext db,
              IMediator mediator) : base(currentDateService, currentUserProvider)
            {
                this.db = db;
                this.mediator = mediator;
            }

            protected override async Task<ItemList<ErrandDto>> Handle(RequestContext context, GetErrandListQuery request, CancellationToken cancellationToken)
            {
                if (context.CurrentUser.Role == UserRole.Employee)
                    return await mediator.Send(new GetEmployeeErrandListQuery(), cancellationToken);

                var query = GetErrandsQuery();

                var totalCount = await query.CountAsync(cancellationToken);

                var errands = await query
                    .Skip(request.Skip ?? 0)
                    .Take(request.Take ?? 10)
                    .ToListAsync(cancellationToken: cancellationToken);

                var points = errands.SelectMany(e => e.Points).Select(p => p.Point.Id).Distinct().ToList();

                var values = await db.PointFieldValues
                    .Where(c => points.Contains(c.PointId))
                    .AsNoTracking()
                    .ToListAsync(cancellationToken: cancellationToken);

                var lastValueBucket = new LastValueBucket(values);

                var items = new List<ErrandDto>();

                foreach(var group in errands.GroupBy(e => new { e.FacilityId, e.EmployeeId }))
                {
                    var notExpired = group.Where(e => e.Expired(context.CurrentDateTime) == false);
                    items.AddRange(notExpired.OrderByDescending(e => e.DueDate).Select(e => ErrandDto.Map(e, context.CurrentUser, context.CurrentDateTime, lastValueBucket, false)));

                    var expired = group.Except(notExpired).OrderByDescending(e => e.DueDate);

                    var shift = notExpired.Any(e => e.DueDate.Date == context.CurrentDateTime.Date) ? 0 : 1;

                    items.AddRange(expired.OrderByDescending(e => e.DueDate).Take(shift).Select(e => ErrandDto.Map(e, context.CurrentUser, context.CurrentDateTime, lastValueBucket, false)));
                    items.AddRange(expired.OrderByDescending(e => e.DueDate).Skip(shift).Select(e => ErrandDto.Map(e, context.CurrentUser, context.CurrentDateTime, lastValueBucket, true)));
                }

                return new ItemList<ErrandDto> 
                { 
                    Items = items, 
                    TotalCount = totalCount
                };
            }

            private IQueryable<Errand> GetErrandsQuery()
            {
                return db
                  .Errands
                  .Include(e => e.Employee)
                  .Include(e => e.Facility)
                  .ThenInclude(f => f.Perimeters)
                  .Include(p => p.Points)
                  .ThenInclude(p => p.Point)
                  .ThenInclude(f => f.Trap)
                  .ThenInclude(r => r.Fields)
                  .OrderByDescending(e => e.DueDate)
                  .AsNoTracking();
            }
        }
    }
}
