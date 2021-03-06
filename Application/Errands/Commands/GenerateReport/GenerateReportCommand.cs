using Application.Common.Interfaces;
using Application.Resources;
using Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Microsoft.EntityFrameworkCore;
using Application.Common.Exceptions;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Application.Common;
using Application.Common.Models;

namespace Application.Errands.Commands.GenerateReport
{
    public record GenerateReportCommand : IRequest<byte[]>
    {
        public GenerateReportCommand(Guid errandId)
        {
            ErrandId = errandId;
        }

        public Guid ErrandId { get; }

        public class GenerateReportCommandHandler : BaseRequestHandler<GenerateReportCommand, byte[]>
        {
            private readonly IDeratControlDbContext db;
            private readonly IReportBuilder reportBuilder;
            private readonly IStringLocalizer<SharedResource> localizer;
            private readonly IFileStorage fileStorage;

            public GenerateReportCommandHandler(
                ICurrentUserProvider currentUserProvider,
                ICurrentDateService currentDateService,
                IDeratControlDbContext db,
                IReportBuilder reportBuilder,
                IStringLocalizer<SharedResource> localizer,
                IFileStorage fileStorage) : base(currentDateService, currentUserProvider)
            {
                this.db = db;
                this.reportBuilder = reportBuilder;
                this.localizer = localizer;
                this.fileStorage = fileStorage;
            }

            protected override async Task<byte[]> Handle(RequestContext context, GenerateReportCommand request, CancellationToken cancellationToken)
            {
                var errand = await GetErrand(request.ErrandId, cancellationToken) ?? throw new NotFoundException("Завдання не знайдено.");
                var providerSignature = await fileStorage.ReadFile(Path.Combine("signatures", context.CurrentUser.UserId.ToString()));
                var providerSeal = await fileStorage.ReadFile(Path.Combine("seals", context.CurrentUser.UserId.ToString()));

                reportBuilder
                    .AddVerticalSpace()
                    .AddText(errand.Provider.ProviderName, Align.Center, 32)
                    .AddSignature(providerSignature)
                    .AddSeal(providerSeal)
                    .AddVerticalSpace()
                    .AddVerticalSpace()
                    .AddText(errand.Facility.GetInfo(), Align.Center, 24)
                    .AddVerticalSpace()
                    .AddText(errand.Description, Align.Center, 20)
                    .AddVerticalSpace();

                foreach (var grouped in errand.PointReviewHistory
                    .OrderBy(p => p.PerimeterId)
                    .ThenBy(p => p.TrapId)
                    .GroupBy(p => new { p.Perimeter, p.Trap, p.Supplement }))
                {
                    reportBuilder
                               .AddVerticalSpace()
                               .AddText($"Периметр - {grouped.Key.Perimeter.PerimeterName}, {grouped.Key.Trap.TrapName} - {grouped.Key.Supplement.SupplementName}", Align.Center, 20)
                               .AddVerticalSpace();

                    var tableColumns = new List<Column> { new Column(1, "№ п/п", grouped.Key.Trap.Color) };
                    tableColumns.AddRange(grouped.Key.Trap.Fields.OrderBy(f => f.Order).Select(f => new Column(f.Order + 1, f.FieldName)));

                    var table = new Table(tableColumns);

                    foreach (var point in grouped.OrderBy(p => p.PointOrder))
                    {
                        Row row = table.NewRow();
                        row[tableColumns[0]] = point.PointOrder.ToString();

                        foreach (var record in point.Records)
                        {
                            row[tableColumns[record.Field.Order]] = record.GetValue();
                        }
                    }

                    reportBuilder
                        .AddTable(table)
                        .AddVerticalSpace()
                        .AddVerticalSpace();
                }

                return reportBuilder.GetReport();
            }

            private Task<CompletedErrand> GetErrand(Guid errandId, CancellationToken cancellationToken)
            {
                return db
                    .CompletedErrands
                    .Include(c => c.Facility)
                    .Include(c => c.Employee)
                    .Include(c => c.Provider)
                    .Include(e => e.PointReviewHistory)
                    .ThenInclude(p => p.Perimeter)
                    .Include(e => e.PointReviewHistory)
                    .ThenInclude(p => p.Trap)
                    .ThenInclude(t => t.Fields)
                    .Include(e => e.PointReviewHistory)
                    .ThenInclude(e => e.Supplement)
                    .Include(e => e.PointReviewHistory)
                    .ThenInclude(p => p.Records)
                    .FirstOrDefaultAsync(e => e.Id == errandId, cancellationToken: cancellationToken);
            }
        }
    }
}
