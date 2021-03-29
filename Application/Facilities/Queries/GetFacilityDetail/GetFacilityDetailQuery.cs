using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Facilities.Queries.GetFacilityDetail {
  public class GetFacilityDetailQuery : IRequest<FacilityDetailVm> {
    public GetFacilityDetailQuery(Guid facilityId) {
      FacilityId = facilityId;
    }

    public Guid FacilityId { get; }

    public class GetFacilityDetailQueryHandler : IRequestHandler<GetFacilityDetailQuery, FacilityDetailVm> {
      private readonly IDeratControlDbContext context;
      private readonly IMapper mapper;

      public GetFacilityDetailQueryHandler(IDeratControlDbContext context, IMapper mapper) {
        this.context = context;
        this.mapper = mapper;
      }

      public async Task<FacilityDetailVm> Handle(GetFacilityDetailQuery request, CancellationToken cancellationToken) {
        var facility = await context
          .Facilities
          .ProjectTo<FacilityDetailVm>(mapper.ConfigurationProvider)
          .FirstOrDefaultAsync(f => f.FacilityId == request.FacilityId);

        if (facility == null)
          throw new NotFoundException();

        return facility;
      }
    }
  }
}
