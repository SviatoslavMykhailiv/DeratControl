using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Traps.Commands.UpsertTrap {
  public class UpsertTrapCommand : IRequest<Guid> {
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }
    public IReadOnlyCollection<FieldDto> Fields { get; init; } = new List<FieldDto>();

    public class UpsertTrapCommandHandler : IRequestHandler<UpsertTrapCommand, Guid> {
      private readonly IDeratControlDbContext context;
      private readonly IMemoryCache cache;

      public UpsertTrapCommandHandler(IDeratControlDbContext context, IMemoryCache cache) {
        this.context = context;
        this.cache = cache;
      }

      public async Task<Guid> Handle(UpsertTrapCommand request, CancellationToken cancellationToken) {

        Trap trap;

        if (request.Id.HasValue) {
          trap = await context.Traps.FindAsync(new object[] { request.Id.Value }, cancellationToken);
        }
        else {
          trap = new Trap();
          context.Traps.Add(trap);
        }

        trap.TrapName = request.Name;
        trap.Color = request.Color;

        var inputFieldList = request.Fields.Where(f => f.FieldId.HasValue).ToDictionary(f => f.FieldId.Value);

        foreach (var field in trap.Fields.ToList()) {
          if (inputFieldList.ContainsKey(field.Id))
            continue;

          trap.RemoveField(field.Id);
        }

        var existingFieldList = trap.Fields.ToDictionary(f => f.Id);

        foreach (var inputField in request.Fields)
          trap.SetField(
            inputField.FieldId,
            inputField.FieldName,
            inputField.Order,
            inputField.FieldType,
            inputField.OptionList);

        await context.SaveChangesAsync(cancellationToken);

        cache.Remove(nameof(Trap));

        return trap.Id;
      }
    }
  }
}
