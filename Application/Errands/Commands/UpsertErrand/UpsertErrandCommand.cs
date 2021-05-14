using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Errands.Commands.UpsertErrand {
  public class UpsertErrandCommand : IRequest<Guid> {
    public UpsertErrandCommand() {
      Points = new HashSet<Guid>();
    }

    public Guid? ErrandId { get; init; }
    public Guid FacilityId { get; init; }
    public Guid EmployeeId { get; init; }
    public DateTime DueDate { get; init; }
    public string Description { get; init; }
    public IReadOnlyCollection<Guid> Points { get; init; } = new List<Guid>();

    public class UpsertErrandCommandHandler : IRequestHandler<UpsertErrandCommand, Guid> {
      private readonly IDeratControlDbContext context;
      private readonly ICurrentDateService currentDateService;
      private readonly IUserManagerService userManagerService;

      public UpsertErrandCommandHandler(
        IDeratControlDbContext context,
        ICurrentDateService currentDateService,
        IUserManagerService userManagerService) {

        this.context = context;
        this.currentDateService = currentDateService;
        this.userManagerService = userManagerService;
      }

      public async Task<Guid> Handle(UpsertErrandCommand request, CancellationToken cancellationToken) {

        if (request.ErrandId is null && await ErrandExists(request.FacilityId, request.EmployeeId, request.DueDate))
          throw new BadRequestException();

        Errand errand;

        if (request.ErrandId.HasValue) {
          errand = await context
            .Errands
            .Include(e => e.Points)
            .Include(c => c.Employee)
            .Include(c => c.Facility)
            .ThenInclude(c => c.Perimeters)
            .ThenInclude(c => c.Points)
            .ThenInclude(p => p.Trap)
            .ThenInclude(t => t.Fields)
            .FirstOrDefaultAsync(c => c.Id == request.ErrandId.Value && c.Status != ErrandStatus.Finished, cancellationToken: cancellationToken) ?? throw new NotFoundException();
        }
        else {
          errand = new Errand();
          context.Errands.Add(errand);
        }

        if (request.DueDate.Date < currentDateService.CurrentDate.Date)
          throw new BadRequestException();

        if (errand.EmployeeId != request.EmployeeId)
          errand.Employee = await userManagerService.GetUser(request.EmployeeId, cancellationToken) ?? throw new NotFoundException();

        if (errand.FacilityId != request.FacilityId)
          errand.Facility = await GetFacility(request.FacilityId, cancellationToken) ?? throw new NotFoundException();

        errand.Description = request.Description;
        errand.SetDueDate(request.DueDate);
        errand.SetPointListForReview(request.Points);

        await context.SaveChangesAsync(cancellationToken);

        return errand.Id;
      }

      private Task<bool> ErrandExists(
        Guid facilityId,
        Guid employeeId,
        DateTime dueDate) {
        return context
          .Errands
          .AnyAsync(e => e.FacilityId == facilityId && e.EmployeeId == employeeId && e.DueDate.Date == dueDate.Date);
      }

      private async Task<Facility> GetFacility(Guid facilityId, CancellationToken cancellationToken) {
        return await context
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
