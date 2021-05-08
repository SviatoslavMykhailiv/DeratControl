using Application.Common;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
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

    public class GetFacilityDetailQueryHandler : BaseRequestHandler<GetFacilityDetailQuery, FacilityDetailVm> {
      private readonly IDeratControlDbContext db;
      private readonly IMapper mapper;

      public GetFacilityDetailQueryHandler(ICurrentUserProvider currentUserProvider, ICurrentDateService currentDateService, IDeratControlDbContext db, IMapper mapper) : base(currentDateService, currentUserProvider) {
        this.db = db;
        this.mapper = mapper;
      }

      protected override async Task<FacilityDetailVm> Handle(RequestContext context, GetFacilityDetailQuery request, CancellationToken cancellationToken) {
        var facility = await db
          .Facilities
          .AsNoTracking()
          .ProjectTo<FacilityDetailVm>(mapper.ConfigurationProvider)
          .FirstOrDefaultAsync(f => f.FacilityId == request.FacilityId, cancellationToken: cancellationToken) ?? throw new NotFoundException();

        if (context.CurrentUser.Role == UserRole.Employee)
          facility.Obfuscate();

        return facility;
      }
    }
  }
}
