using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Traps.Commands.UpsertTrap {
  public class UpsertTrapCommand : IRequest<Guid> {
    public Guid? TrapId { get; set; }
    public string TrapName { get; set; }
    public string Color { get; set; }

    public class UpsertTrapCommandHandler : IRequestHandler<UpsertTrapCommand, Guid> {
      private readonly IDeratControlDbContext context;
      private readonly IMemoryCache cache;

      public UpsertTrapCommandHandler(IDeratControlDbContext context, IMemoryCache cache) {
        this.context = context;
        this.cache = cache;
      }

      public async Task<Guid> Handle(UpsertTrapCommand request, CancellationToken cancellationToken) {

        Trap trap;

        if (request.TrapId.HasValue) {
          trap = await context.Traps.FindAsync(new object[] { request.TrapId.Value }, cancellationToken);
        }
        else {
          trap = new Trap();
          context.Traps.Add(trap);
        }

        trap.TrapName = request.TrapName;
        trap.Color = request.Color;

        await context.SaveChangesAsync(cancellationToken);

        cache.Remove(nameof(Trap));

        return trap.TrapId;
      }
    }
  }
}
