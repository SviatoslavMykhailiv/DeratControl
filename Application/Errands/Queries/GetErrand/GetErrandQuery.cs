using Application.Common;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Errands.Queries.GetErrand
{
    public record GetErrandQuery : IRequest<ErrandDto>
    {
        public GetErrandQuery(Guid errandId)
        {
            ErrandId = errandId;
        }

        public Guid ErrandId { get; }

        public class GetErrandQueryHandler : BaseRequestHandler<GetErrandQuery, ErrandDto>
        {
            private readonly IDeratControlDbContext db;

            public GetErrandQueryHandler(
              ICurrentDateService currentDateService,
              ICurrentUserProvider currentUserProvider,
              IDeratControlDbContext db) : base(currentDateService, currentUserProvider)
            {
                this.db = db;
            }

            protected override async Task<ErrandDto> Handle(RequestContext context, GetErrandQuery request, CancellationToken cancellationToken)
            {
                var errand = await GetErrandsQuery(request.ErrandId);

                if (errand is null)
                    throw new NotFoundException("Завдання не знайдено.");

                return ErrandDto.Map(errand, context.CurrentUser, context.CurrentDateTime, new LastValueBucket());
            }

            private Task<Errand> GetErrandsQuery(Guid errandId)
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
                  .AsNoTracking()
                  .FirstOrDefaultAsync(e => e.Id == errandId);
            }
        }
    }
}
