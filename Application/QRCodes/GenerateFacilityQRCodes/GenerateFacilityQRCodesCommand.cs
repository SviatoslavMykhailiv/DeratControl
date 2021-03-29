using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.QRCodes.GenerateFacilityQRCodes {
  public class GenerateFacilityQRCodesCommand : IRequest<byte[]> {
    public ICollection<Guid> Perimeters { get; set; }
    public ICollection<Guid> Traps { get; set; }

    public class GenerateFacilityQRCodesCommandHandler : IRequestHandler<GenerateFacilityQRCodesCommand, byte[]> {
      private readonly IDeratControlDbContext context;

      public GenerateFacilityQRCodesCommandHandler(IDeratControlDbContext context) {
        this.context = context;
      }

      public Task<byte[]> Handle(GenerateFacilityQRCodesCommand request, CancellationToken cancellationToken) {



        return null;
      }
    }
  }
}
