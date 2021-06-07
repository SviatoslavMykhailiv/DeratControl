using Application.Common;
using Application.Common.Dtos;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
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

                return new ItemList<ErrandDto> { Items = errands.Select(e => ErrandDto.Map(e, context.CurrentUser)), TotalCount = totalCount };
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
