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
using System.Collections.Generic;
using System;

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

                var errands = FilterExpired(await query.ToListAsync(cancellationToken: cancellationToken), context.CurrentDateTime)
                    .OrderByDescending(e => e.DueDate)
                    .ToList();

                var points = errands.SelectMany(e => e.Points).Select(p => p.Point.Id).Distinct().ToList();

                var values = await db.PointFieldValues
                    .Where(c => points.Contains(c.PointId))
                    .AsNoTracking()
                    .ToListAsync(cancellationToken: cancellationToken);

                var lastValueBucket = new LastValueBucket(values);

                return new ItemList<ErrandDto> 
                {
                    Items = errands.Select(e => ErrandDto.Map(e, context.CurrentUser, context.CurrentDateTime, lastValueBucket, false)), 
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

            private static IEnumerable<Errand> FilterExpired(List<Errand> errands, DateTime currentDate) 
            {
                var grouped = errands.GroupBy(e => e.Facility);

                foreach (var group in grouped)
                {
                    foreach (var item in group.Where(i => i.DueDate > currentDate))
                        yield return item;

                    var latestExpired = group
                        .Where(i => i.DueDate < currentDate)
                        .OrderByDescending(c => c.DueDate)
                        .FirstOrDefault();

                    if (latestExpired is not null)
                        yield return latestExpired;
                }
            }
        }

    }
}
