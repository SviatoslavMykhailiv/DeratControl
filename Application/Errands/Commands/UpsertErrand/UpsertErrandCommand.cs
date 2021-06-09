using Application.Common;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Errands.Commands.UpsertErrand
{
    public class UpsertErrandCommand : IRequest<Guid>
    {
        public UpsertErrandCommand()
        {
            Points = new HashSet<Guid>();
        }

        public Guid? ErrandId { get; init; }
        public Guid FacilityId { get; init; }
        public Guid EmployeeId { get; init; }
        public DateTime DueDate { get; init; }
        public string Description { get; init; }
        public bool OnDemand { get; init; }

        public IReadOnlyCollection<Guid> Points { get; init; } = new List<Guid>();

        public class UpsertErrandCommandHandler : BaseRequestHandler<UpsertErrandCommand, Guid>
        {
            private readonly IDeratControlDbContext db;
            private readonly IUserManagerService userManagerService;

            public UpsertErrandCommandHandler(
              ICurrentDateService currentDateService,
              ICurrentUserProvider currentUserProvider,
              IDeratControlDbContext db,
              IUserManagerService userManagerService) : base(currentDateService, currentUserProvider)
            {
                this.db = db;
                this.userManagerService = userManagerService;
            }

            protected override async Task<Guid> Handle(RequestContext context, UpsertErrandCommand request, CancellationToken cancellationToken)
            {
                if (request.ErrandId is null && await ErrandExists(request.FacilityId, request.EmployeeId, request.DueDate))
                    throw new BadRequestException("Таке завдання уже існує.");

                Errand errand;

                if (request.ErrandId.HasValue)
                {
                    errand = await db
                      .Errands
                      .Include(e => e.Points)
                      .Include(c => c.Employee)
                      .Include(c => c.Facility)
                      .ThenInclude(c => c.Perimeters)
                      .ThenInclude(c => c.Points)
                      .ThenInclude(p => p.Trap)
                      .ThenInclude(t => t.Fields)
                      .FirstOrDefaultAsync(c => c.Id == request.ErrandId.Value, cancellationToken: cancellationToken) ?? throw new NotFoundException("Завдання не знайдено.");
                }
                else
                {
                    errand = new Errand { ProviderId = context.CurrentUser.UserId };
                    db.Errands.Add(errand);
                }

                if (request.DueDate.Date < context.CurrentDateTime.Date)
                    throw new BadRequestException("Дата завдання не може бути заднім числом.");

                errand.EmployeeId = request.EmployeeId;

                if (errand.FacilityId != request.FacilityId)
                    errand.Facility = await GetFacility(request.FacilityId, cancellationToken) ?? throw new NotFoundException("Об'єкт не знайдено.");

                errand.Description = request.Description;
                errand.SetDueDate(request.DueDate);
                errand.SetPointListForReview(request.Points);
                errand.OnDemand = request.OnDemand;

                await db.SaveChangesAsync(cancellationToken);

                return errand.Id;
            }

            private Task<bool> ErrandExists(
              Guid facilityId,
              Guid employeeId,
              DateTime dueDate)
            {
                return db
                  .Errands
                  .AnyAsync(e => e.FacilityId == facilityId && e.EmployeeId == employeeId && e.DueDate.Date == dueDate.Date);
            }

            private async Task<Facility> GetFacility(Guid facilityId, CancellationToken cancellationToken)
            {
                return await db
                  .Facilities
                  .Include(f => f.Perimeters)
                  .ThenInclude(p => p.Points)
                  .ThenInclude(p => p.Trap)
                  .ThenInclude(t => t.Fields)
                  .FirstOrDefaultAsync(f => f.Id == facilityId, cancellationToken);
            }
        }
    }
}
