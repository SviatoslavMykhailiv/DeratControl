using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Facilities.Queries.GetFacilityList {
  public class GetFacilityListQuery : IRequest<FacilityListVm> {
    public class GetFacilityListQueryHandler : IRequestHandler<GetFacilityListQuery, FacilityListVm> {
      private readonly IDeratControlDbContext context;
      private readonly IMapper mapper;

      public GetFacilityListQueryHandler(IDeratControlDbContext context, IMapper mapper) {
        this.context = context;
        this.mapper = mapper;
      }

      public async Task<FacilityListVm> Handle(GetFacilityListQuery request, CancellationToken cancellationToken) {
        var facilities = await context
          .Facilities
          .ProjectTo<FacilityHeaderDto>(mapper.ConfigurationProvider)
          .ToListAsync(cancellationToken);

        return new FacilityListVm { Facilities = facilities };
      }
    }
  }
}
