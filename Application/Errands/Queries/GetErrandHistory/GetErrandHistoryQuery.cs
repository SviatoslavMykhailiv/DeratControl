using Application.Common.Dtos;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Errands.Queries.GetErrandHistory
{
    public record GetErrandHistoryQuery : IRequest<ItemList<ErrandHistoryDto>>
    {
        public GetErrandHistoryQuery(Guid? facilityId, Guid? employeeId, int skip, int take)
        {
            FacilityId = facilityId;
            EmployeeId = employeeId;
            Skip = skip;
            Take = take;
        }

        public Guid? FacilityId { get; }
        public Guid? EmployeeId { get; }
        public int Skip { get; }
        public int Take { get; }

        public class GetErrandHistoryQueryHandler : IRequestHandler<GetErrandHistoryQuery, ItemList<ErrandHistoryDto>>
        {
            private readonly IDeratControlDbContext db;

            public GetErrandHistoryQueryHandler(IDeratControlDbContext db)
            {
                this.db = db;
            }

            public async Task<ItemList<ErrandHistoryDto>> Handle(GetErrandHistoryQuery request, CancellationToken cancellationToken)
            {
                var query = db.CompletedErrands
                    .Include(c => c.Facility)
                    .Include(c => c.Employee)
                    .AsQueryable();

                if (request.EmployeeId.HasValue)
                    query = query.Where(e => e.EmployeeId == request.EmployeeId.Value);

                if (request.FacilityId.HasValue)
                    query = query.Where(e => e.FacilityId == request.FacilityId.Value);

                var totalCount = await query.CountAsync(cancellationToken);

                var result = await query
                    .OrderByDescending(e => e.CompleteDate)
                    .Skip(request.Skip)
                    .Take(request.Take)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                return new ItemList<ErrandHistoryDto> { Items = result.Select(e => ErrandHistoryDto.Map(e)), TotalCount = totalCount };
            }
        }
    }
}
