using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Points.Queries.PointReviewHistory
{
    public record PointReviewHistoryQuery : IRequest<IEnumerable<PointReviewHistoryRecord>>
    {
        public PointReviewHistoryQuery(Guid pointId)
        {
            PointId = pointId;
        }

        public Guid PointId { get; }

        public class PointReviewHistoryQueryHandler : IRequestHandler<PointReviewHistoryQuery, IEnumerable<PointReviewHistoryRecord>>
        {
            private readonly IDeratControlDbContext db;

            public PointReviewHistoryQueryHandler(IDeratControlDbContext db)
            {
                this.db = db;
            }

            public async Task<IEnumerable<PointReviewHistoryRecord>> Handle(PointReviewHistoryQuery request, CancellationToken cancellationToken)
            {
                var reviewHistory = await db.CompletedPointReviews
                    .Include(c => c.Records)
                    .ThenInclude(r => r.Field)
                    .Include(c => c.Errand)
                    .ThenInclude(e => e.Employee)
                    .Where(p => p.PointId == request.PointId)
                    .ToListAsync(cancellationToken: cancellationToken);

                var history = new List<PointReviewHistoryRecord>();

                foreach (var point in reviewHistory.OrderByDescending(r => r.Errand.CompleteDate))
                {
                    var record = new PointReviewHistoryRecord
                    {
                        ReviewDate = point.Errand.CompleteDate.ToShortDateString(),
                        Employee = point.Errand.Employee.GetFullName(),
                        Records = point.Records.Select(r => new PointRecord
                        {
                            FieldName = r.Field.FieldName,
                            Value = r.Field.ToStringValue(r.Value)
                        }).OrderBy(r => r.FieldName).ToList()
                    };

                    history.Add(record);
                }

                return history;
            }
        }
    }
}
