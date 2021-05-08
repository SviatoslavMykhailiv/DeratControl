using Application.Common;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Errands.Commands.MoveErrand {
  public class MoveErrandCommand : IRequest {
    public Guid ErrandId { get; init; }

    public class MoveErrandCommandHandler : BaseRequestHandler<MoveErrandCommand> {
      private readonly IDeratControlDbContext db;
      private readonly ICurrentDateService currentDateService;

      public MoveErrandCommandHandler(
        IDeratControlDbContext db,
        ICurrentDateService currentDateService, 
        ICurrentUserProvider currentUserProvider) : base(currentDateService, currentUserProvider) {
        this.currentDateService = currentDateService;
        this.db = db;
      }

      protected override async Task<Unit> Handle(RequestContext context, MoveErrandCommand request, CancellationToken cancellationToken) {
        var errand = await db.Errands.FirstOrDefaultAsync(e => e.Id == request.ErrandId && e.Status != ErrandStatus.Finished, cancellationToken: cancellationToken) ?? throw new NotFoundException();
        errand.MoveDueDate(currentDateService.CurrentDate);

        await db.SaveChangesAsync(cancellationToken);

        return Unit.Value;
      }
    }

  }
}
