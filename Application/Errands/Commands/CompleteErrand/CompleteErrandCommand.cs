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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Errands.Commands.CompleteErrand {
  public class CompleteErrandCommand : IRequest {
    public Guid ErrandId { get; init; }
    public string Report { get; init; }
    public string SecurityCode { get; init; }
    public byte[] Signature { get; init; }
    public IReadOnlyCollection<PointReviewDto> Points { get; init; } = new List<PointReviewDto>();

    public class CompleteErrandCommandHandler : BaseRequestHandler<CompleteErrandCommand, Unit> {
      private readonly IDeratControlDbContext db;
      private readonly ICurrentDateService currentDateService;

      public CompleteErrandCommandHandler(
        ICurrentUserProvider currentUserProvider,
        IDeratControlDbContext db, 
        ICurrentDateService currentDateService) : base(currentDateService, currentUserProvider) {
        this.db = db;
        this.currentDateService = currentDateService;
      }

      protected override async Task<Unit> Handle(RequestContext context, CompleteErrandCommand request, CancellationToken cancellationToken) {
        var errand = await GetErrand(request.ErrandId, context.CurrentUser.UserId) ?? throw new NotFoundException();
        var incomingPointList = request.Points.ToDictionary(p => p.PointId);

        if (errand.IsSecurityCodeValid(request.SecurityCode) == false)
          throw new BadRequestException();

        foreach(var point in errand.Points) {
          var pointData = incomingPointList.GetValueOrDefault(point.PointId);

          if (pointData is null) 
            continue;

          point.Complete(pointData.Status, pointData.Report, pointData.GetValueList());
        }

        errand.Complete(currentDateService.CurrentDate, request.Report);

        await db.SaveChangesAsync(cancellationToken);

        return Unit.Value;
      }

      private Task<Errand> GetErrand(Guid errandId, Guid employeeId) {
        return db
          .Errands
          .Include(e => e.Facility)
          .Include(e => e.Points)
          .ThenInclude(p => p.Records)
          .ThenInclude(r => r.Field)
          .FirstOrDefaultAsync(e => e.Id == errandId && e.EmployeeId == employeeId && e.Status == ErrandStatus.Planned);
      }

    }

  }
}
