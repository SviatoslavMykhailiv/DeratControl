using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Facilities.Queries.GetFacilityList
{
    public class GetFacilityListQuery : IRequest<IEnumerable<FacilityHeaderDto>>
    {
        public class GetFacilityListQueryHandler : BaseRequestHandler<GetFacilityListQuery, IEnumerable<FacilityHeaderDto>>
        {
            private readonly IDeratControlDbContext db;
            private readonly IMapper mapper;

            public GetFacilityListQueryHandler(
              ICurrentUserProvider currentUserProvider,
              IDeratControlDbContext db,
              IMapper mapper,
              ICurrentDateService currentDateService) : base(currentDateService, currentUserProvider)
            {
                this.db = db;
                this.mapper = mapper;
            }

            protected override async Task<IEnumerable<FacilityHeaderDto>> Handle(RequestContext context, GetFacilityListQuery request, CancellationToken cancellationToken)
            {
                return await (context.CurrentUser.Role switch
                {
                    UserRole.Employee => (from facility in db.Facilities.AsNoTracking()
                                          join errand in db.Errands.AsNoTracking()
                                          on facility.Id equals errand.FacilityId
                                          where errand.EmployeeId == context.CurrentUser.UserId
                                          select facility).Distinct(),
                    UserRole.Customer => db.Facilities.AsNoTracking().Where(f => f.Id == context.CurrentUser.FacilityId.Value),
                    _ => db.Facilities.AsNoTracking().AsQueryable(),
                }).ProjectTo<FacilityHeaderDto>(mapper.ConfigurationProvider).ToListAsync(cancellationToken);
            }
        }
    }
}
