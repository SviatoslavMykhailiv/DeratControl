using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Errands.Commands.MoveErrand;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.Enums;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper.QueryableExtensions;
using System;

namespace Application.Errands.Queries {
  public record GetEmployeeErrandListQuery : IRequest<IEnumerable<ErrandDto>> {

    public class GetEmployeeErrandListQueryHandler : BaseRequestHandler<GetEmployeeErrandListQuery, IEnumerable<ErrandDto>> {
      private readonly IDeratControlDbContext db;
      private readonly IMapper mapper;
      private readonly IMediator mediator;

      public GetEmployeeErrandListQueryHandler(
        IDeratControlDbContext db,
        IMapper mapper,
        ICurrentDateService currentDateService,
        ICurrentUserProvider currentUserProvider,
        IMediator mediator) : base(currentDateService, currentUserProvider) {
        this.db = db;
        this.mapper = mapper;
        this.mediator = mediator;
      }

      protected override async Task<IEnumerable<ErrandDto>> Handle(RequestContext context, GetEmployeeErrandListQuery request, CancellationToken cancellationToken) {
        var errands = await db.Errands
          .Where(e => e.EmployeeId == context.CurrentUser.UserId && e.Status != ErrandStatus.Finished)
          .AsNoTracking()
          .ProjectTo<ErrandDto>(mapper.ConfigurationProvider)
          .ToListAsync(cancellationToken: cancellationToken);

        foreach (var errand in errands.Where(e => Convert.ToDateTime(e.DueDate) < context.CurrentDateTime))
          await mediator.Send(new MoveErrandCommand { ErrandId = errand.ErrandId }, cancellationToken);

        return errands.Select(e => mapper.Map<ErrandDto>(e)).ToList();
      }
    }

  }
}
