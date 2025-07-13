using Application.Common;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Errands.Commands.CompleteErrand
{
    public class CompleteErrandCommand : IRequest
    {
        public Guid ErrandId { get; init; }
        public string Report { get; init; }
        public string SecurityCode { get; init; }
        public string Signature { get; init; }
        public IReadOnlyCollection<PointReviewDto> Points { get; init; } = new List<PointReviewDto>();

        public class CompleteErrandCommandHandler : BaseRequestHandler<CompleteErrandCommand>
        {
            private readonly IDeratControlDbContext db;
            private readonly ICurrentDateService currentDateService;
            private readonly IFileStorage fileStorage;

            public CompleteErrandCommandHandler(
              ICurrentUserProvider currentUserProvider,
              IDeratControlDbContext db,
              ICurrentDateService currentDateService,
              IFileStorage fileStorage) : base(currentDateService, currentUserProvider)
            {
                this.db = db;
                this.currentDateService = currentDateService;
                this.fileStorage = fileStorage;
            }

            protected override async Task Handle(RequestContext context, CompleteErrandCommand request, CancellationToken cancellationToken)
            {
                var errand = await GetErrand(request.ErrandId, context.CurrentUser.UserId) ?? throw new NotFoundException("Завдання не знайдено.");
                var incomingPointList = request.Points.ToDictionary(p => p.PointId);

                if (errand.IsSecurityCodeValid(request.SecurityCode) == false)
                    throw new BadRequestException("Захисний код невірний.");

                var completedPointReviewList = new List<CompletedPointReview>();

                foreach (var point in errand.Points)
                {
                    var pointData = incomingPointList.GetValueOrDefault(point.PointId);

                    if (pointData is null)
                        continue;

                    var completedReview = point.Complete(pointData.GetValueList());

                    completedPointReviewList.Add(completedReview);
                }

                var completedErrand = errand.Complete(
                    currentDateService.CurrentDate,
                    request.Report,
                    completedPointReviewList);

                var image = (Image)request.Signature;

                if (image is not null)
                    await fileStorage.SaveFile(errand.GetManagerSignatureFilePath(), image);

                db.Errands.Remove(errand);
                db.CompletedErrands.Add(completedErrand);

                await db.SaveChangesAsync(cancellationToken);
            }

            private Task<Errand> GetErrand(Guid errandId, Guid employeeId)
            {
                return db
                  .Errands
                  .Include(e => e.Facility)
                  .ThenInclude(p => p.Perimeters)
                  .ThenInclude(p => p.Points)
                  .ThenInclude(p => p.Supplement)
                  .Include(e => e.Employee)
                  .Include(e => e.Provider)
                  .Include(e => e.Points)
                  .ThenInclude(p => p.Point)
                  .ThenInclude(p => p.Trap)
                  .ThenInclude(p => p.Fields)
                  .Include(e => e.Points)
                  .ThenInclude(p => p.Point)
                  .ThenInclude(p => p.Values)
                  .FirstOrDefaultAsync(e => e.Id == errandId && e.EmployeeId == employeeId);
            }

        }

    }
}
