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

    public Guid? ErrandId { get; set; }
    public Guid FacilityId { get; set; }
    public Guid EmployeeId { get; set; }
    public DateTime DueDate { get; set; }
    public string Description { get; set; }
    public IReadOnlyCollection<Guid> Points { get; set; }

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

        if (await ErrandExists(request.FacilityId, request.EmployeeId, request.DueDate))
          throw new BadRequestException();

        Errand errand;

        if(request.ErrandId.HasValue) {
          errand = await context
            .Errands
            .Include(c => c.Employee)
            .Include(c => c.Facility)
            .ThenInclude(c => c.Perimeters)
            .ThenInclude(c => c.Points)
            .ThenInclude(p => p.Supplement)
            .ThenInclude(s => s.Fields)
            .Include(c => c.Reviews)
            .FirstOrDefaultAsync(c => c.ErrandId == request.ErrandId.Value) ?? throw new NotFoundException();
        }
        else {
          errand = new Errand();
          context.Errands.Add(errand);
        }

        if (errand.Status == ErrandStatus.Finished)
          return errand.ErrandId;

        if (request.DueDate < currentDateService.CurrentDate)
          throw new BadRequestException();

        var employee = await userManagerService.GetUser(request.EmployeeId) ?? throw new NotFoundException();
        var facility = await GetFacility(request.FacilityId, cancellationToken);

        errand.Facility = facility;
        errand.Employee = employee;
        errand.Description = request.Description;
        errand.DueDate = request.DueDate;
        errand.OriginalDueDate = request.DueDate;
        errand.Status = ErrandStatus.Planned;

        errand.SetPointReview(request.Points, await GetSupplements());

        return errand.EmployeeId;
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
          .ThenInclude(p => p.Supplement)
          .ThenInclude(s => s.Fields)
          .FirstOrDefaultAsync(f => f.FacilityId == facilityId, cancellationToken) ?? throw new NotFoundException();
      }

      private async Task<IDictionary<Guid, Supplement>> GetSupplements() {
        return await context
          .Supplements
          .Include(s => s.Fields)
          .ToDictionaryAsync(c => c.SupplementId);
      }
    }
  }
}
