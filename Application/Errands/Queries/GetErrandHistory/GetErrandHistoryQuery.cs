using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Errands.Queries.GetErrandHistory {
  public record GetErrandHistoryQuery : IRequest<IEnumerable<ErrandHistoryDto>> {
    public GetErrandHistoryQuery(Guid? facilityId, Guid? employeeId) {
      FacilityId = facilityId;
      EmployeeId = employeeId;
    }

    public Guid? FacilityId { get; }
    public Guid? EmployeeId { get; }

    public class GetErrandHistoryQueryHandler : IRequestHandler<GetErrandHistoryQuery, IEnumerable<ErrandHistoryDto>> {
      private readonly IDeratControlDbContext db;

      public GetErrandHistoryQueryHandler(IDeratControlDbContext db) {
        this.db = db;
      }

      public async Task<IEnumerable<ErrandHistoryDto>> Handle(GetErrandHistoryQuery request, CancellationToken cancellationToken) {

        var query = GetErrandsQuery();

        if (request.EmployeeId.HasValue)
          query = query.Where(e => e.EmployeeId == request.EmployeeId.Value);

        if (request.FacilityId.HasValue)
          query = query.Where(e => e.FacilityId == request.FacilityId.Value);

        var result = await query.ToListAsync(cancellationToken);

        return result.Select(e => ErrandHistoryDto.Map(e));
      }

      private IQueryable<Errand> GetErrandsQuery() {
        return db.Errands
          .Include(e => e.Employee)
          .Include(e => e.Facility)
          .ThenInclude(f => f.Perimeters)
          .ThenInclude(p => p.Points)
          .ThenInclude(p => p.Supplement)
          .Include(p => p.Points)
          .ThenInclude(p => p.Point.Trap);
      }
    }
  }
}
