using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Perimeters.Queries.GetPerimeterSchemeImage
{
    public record GetPerimeterSchemeImageQuery : IRequest<byte[]>
    {
        public GetPerimeterSchemeImageQuery(Guid perimeterId)
        {
            PerimeterId = perimeterId;
        }

        public Guid PerimeterId { get; }

        public class GetPerimeterSchemeImageQueryHandler : IRequestHandler<GetPerimeterSchemeImageQuery, byte[]>
        {
            private readonly IDeratControlDbContext db;
            private readonly IFileStorage fileStorage;

            public GetPerimeterSchemeImageQueryHandler(IDeratControlDbContext db, IFileStorage fileStorage)
            {
                this.db = db;
                this.fileStorage = fileStorage;
            }

            public async Task<byte[]> Handle(GetPerimeterSchemeImageQuery request, CancellationToken cancellationToken)
            {
                var perimeter = await GetPerimeter(request.PerimeterId) ?? throw new NotFoundException("Периметр не знайдено.");
                var scheme = await fileStorage.ReadFile(perimeter.SchemeImagePath);
                return perimeter.GeneratePerimeterImage(scheme);
            }

            private Task<Perimeter> GetPerimeter(Guid perimeterId)
            {
                return db.Perimeters.Include(p => p.Points).ThenInclude(p => p.Trap).FirstOrDefaultAsync(p => p.Id == perimeterId);
            }

        }
    }
}
