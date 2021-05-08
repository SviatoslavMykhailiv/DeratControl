using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Errands.Queries {
  public class GetErrandListQuery : IRequest<IEnumerable<ErrandDto>> {
    public class GetErrandListQueryHandler : BaseRequestHandler<GetErrandListQuery, IEnumerable<ErrandDto>> {
      private readonly IDeratControlDbContext db;
      private readonly IMapper mapper;
      private readonly IMediator mediator;

      public GetErrandListQueryHandler(
        ICurrentUserProvider currentUserProvider, 
        ICurrentDateService currentDateService, 
        IDeratControlDbContext db,
        IMapper mapper,
        IMediator mediator) : base(currentDateService, currentUserProvider) {
        this.db = db;
        this.mapper = mapper;
        this.mediator = mediator;
      }

      protected override async Task<IEnumerable<ErrandDto>> Handle(RequestContext context, GetErrandListQuery request, CancellationToken cancellationToken) {
        if (context.CurrentUser.Role == UserRole.Customer)
          return await mediator.Send(new GetCustomerErrandListQuery(), cancellationToken);

        if (context.CurrentUser.Role == UserRole.Employee)
          return await mediator.Send(new GetEmployeeErrandListQuery(), cancellationToken);

        var errands = await db
          .Errands
          .AsNoTracking()
          .ProjectTo<ErrandDto>(mapper.ConfigurationProvider)
          .ToListAsync(cancellationToken: cancellationToken);

        return errands;
      }
    }
  }
}
