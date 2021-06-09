using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Perimeters.Queries.GetPerimeterDetail
{
    public class GetPerimeterDetailQuery : IRequest<PerimeterVm>
    {

        public GetPerimeterDetailQuery(Guid perimeterId)
        {
            PerimeterId = perimeterId;
        }

        public Guid PerimeterId { get; }

        public class GetPerimeterDetailQueryHandler : IRequestHandler<GetPerimeterDetailQuery, PerimeterVm>
        {
            private readonly IDeratControlDbContext context;
            private readonly IMapper mapper;

            public GetPerimeterDetailQueryHandler(IDeratControlDbContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<PerimeterVm> Handle(GetPerimeterDetailQuery request, CancellationToken cancellationToken)
            {
                var perimeter = await context
                  .Perimeters
                  .AsNoTracking()
                  .ProjectTo<PerimeterVm>(mapper.ConfigurationProvider)
                  .FirstOrDefaultAsync(p => p.PerimeterId == request.PerimeterId, cancellationToken) ?? throw new NotFoundException("Периметр не знайдено.");

                return perimeter;
            }
        }
    }
}
