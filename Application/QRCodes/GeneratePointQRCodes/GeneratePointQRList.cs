using Application.Common.Dtos;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.QRCodes.GeneratePointQRCodes
{
    public record GeneratePointQRListCommand : IRequest<byte[]>
    {
        public Guid PerimeterId { get; init; }
        public IReadOnlyCollection<PointQRDto> Points { get; init; } = new List<PointQRDto>();

        public class GeneratePointQRListCommandHandler : IRequestHandler<GeneratePointQRListCommand, byte[]>
        {
            private readonly IDeratControlDbContext db;
            private readonly IQRListGenerator qrListGenerator;

            public GeneratePointQRListCommandHandler(IDeratControlDbContext db, IQRListGenerator qrListGenerator)
            {
                this.db = db;
                this.qrListGenerator = qrListGenerator;
            }

            public async Task<byte[]> Handle(GeneratePointQRListCommand request, CancellationToken cancellationToken)
            {
                var perimeter = await GetPerimeter(request.PerimeterId, cancellationToken) ?? throw new NotFoundException("Периметр не знайдено.");
                var traps = await GetTrapList(request.Points.Select(p => p.TrapId).Distinct().ToArray(), cancellationToken);
                return qrListGenerator.Generate(perimeter, request.Points, traps);
            }

            private async Task<Perimeter> GetPerimeter(Guid perimeterId, CancellationToken cancellationToken)
            {
                return await db
                  .Perimeters
                  .Include(p => p.Facility)
                  .Include(p => p.Points)
                  .ThenInclude(p => p.Trap)
                  .FirstOrDefaultAsync(p => p.Id == perimeterId, cancellationToken);
            }

            private async Task<Dictionary<Guid, Trap>> GetTrapList(IReadOnlyCollection<Guid> trapIdList, CancellationToken cancellationToken)
            {
                return await db
                  .Traps
                  .Where(t => trapIdList.Contains(t.Id))
                  .AsNoTracking()
                  .ToDictionaryAsync(t => t.Id, cancellationToken);
            }
        }
    }
}
