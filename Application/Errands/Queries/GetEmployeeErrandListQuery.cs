using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Application.Common.Dtos;

namespace Application.Errands.Queries
{
    public record GetEmployeeErrandListQuery : IRequest<ItemList<ErrandDto>>
    {

        public class GetEmployeeErrandListQueryHandler : BaseRequestHandler<GetEmployeeErrandListQuery, ItemList<ErrandDto>>
        {
            private readonly IDeratControlDbContext db;

            public GetEmployeeErrandListQueryHandler(
                IDeratControlDbContext db,
                ICurrentDateService currentDateService,
                ICurrentUserProvider currentUserProvider) : base(currentDateService, currentUserProvider)
            {
                this.db = db;
            }

            protected override async Task<ItemList<ErrandDto>> Handle(RequestContext context, GetEmployeeErrandListQuery request, CancellationToken cancellationToken)
            {
                var query = GetErrandsQuery(context.CurrentUser);

                var errands = await query.ToListAsync(cancellationToken: cancellationToken);

                var points = errands.SelectMany(e => e.Points).Select(p => p.Point.Id).Distinct().ToList();

                var completedPointReviews = await db.CompletedPointReviews
                    .Include(c => c.Records)
                    .Where(c => points.Contains(c.PointId))
                    .AsNoTracking()
                    .ToListAsync(cancellationToken: cancellationToken);

                var lastValueBucket = new LastValueBucket(completedPointReviews);

                return new ItemList<ErrandDto> 
                {
                    Items = errands.Select(e => ErrandDto.Map(e, context.CurrentUser, context.CurrentDateTime, lastValueBucket)), 
                    TotalCount = errands.Count 
                };
            }

            private IQueryable<Errand> GetErrandsQuery(CurrentUser user)
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
                  .Where(e => e.EmployeeId == user.UserId)
                  .OrderByDescending(e => e.DueDate)
                  .Take(5)
                  .AsNoTracking();
            }
        }

    }
}
