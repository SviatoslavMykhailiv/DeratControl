using Application.Common.Interfaces;
using Application.Resources;
using Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Microsoft.EntityFrameworkCore;

namespace Application.Errands.Commands.GenerateReport
{
    public record GenerateReportCommand : IRequest<byte[]>
    {
        public GenerateReportCommand(Guid errandId)
        {
            ErrandId = errandId;
        }

        public Guid ErrandId { get; }

        public class GenerateReportCommandHandler : IRequestHandler<GenerateReportCommand, byte[]>
        {
            private readonly IDeratControlDbContext db;
            private readonly IReportBuilder reportBuilder;
            private readonly IStringLocalizer<SharedResource> localizer;
            private readonly IFileStorage fileStorage;

            public GenerateReportCommandHandler(
              IDeratControlDbContext db,
              IReportBuilder reportBuilder,
              IStringLocalizer<SharedResource> localizer,
              IFileStorage fileStorage)
            {
                this.db = db;
                this.reportBuilder = reportBuilder;
                this.localizer = localizer;
                this.fileStorage = fileStorage;
            }

            public async Task<byte[]> Handle(GenerateReportCommand request, CancellationToken cancellationToken)
            {
                //var errand = await GetErrand(request.ErrandId, cancellationToken) ?? throw new NotFoundException();
                //var managerSignature = await fileStorage.ReadFile(errand.GetManagerSignatureFilePath());
                ////var providerName = errand.Provider.Company == null ? errand.Provider.Name.GetFullName(CultureInfo.CurrentCulture) : errand.Provider.Company.CompanyName;

                //reportBuilder
                //    .AddVerticalSpace()
                //    .AddText("ТОВ Залупа", Align.Center, 32)
                //    .AddVerticalSpace()
                //    .AddText($"{localizer["ReviewDate"].Value}: {errand.CompleteDate.Value.ToString("dd/MM/yyyy", CultureInfo.CurrentCulture)}", Align.Right, 20)
                //    .AddVerticalSpace()
                //    .AddText($"{localizer["ReviewedBy"].Value}: {errand.Employee.GetFullName()}", Align.Right, 20)
                //    .AddVerticalSpace()
                //    .AddText(errand.Description, Align.Center, 24)
                //    .AddVerticalSpace()
                //    .AddText(errand.Facility.CompanyName, Align.Center, 24)
                //    .AddVerticalSpace();

                //var points = errand.Points.OrderBy(p => p.Point.Order).GroupBy(t => t.Point.Trap);

                //var genericColumns = new List<string> { 
                //  localizer["TrapOrder"].Value, 
                //  localizer["SupplementName"].Value, 
                //  localizer["Action"].Value };

                //foreach (var trap in points) {
                //  var tableColumns = new List<string>(genericColumns);
                //  tableColumns.AddRange(trap.Key.Fields.OrderBy(f => f.Order).Select(field => field.FieldName));

                //  tableColumns.Add(localizer["Notes"].Value);

                //  var table = new Table(tableColumns);

                //  foreach (var point in trap) {
                //    var row = table.NewRow();

                //    row[genericColumns[0]] = point.Point.Order.ToString();
                //    row[genericColumns[1]] = point.Point.Supplement.SupplementName;
                //    row[genericColumns[2]] = localizer[point.Status.ToString()].Value;

                //    foreach (var field in trap.Key.Fields.OrderBy(f => f.Order))
                //      row[field.FieldName] = point[field.Id];

                //    row[localizer["Notes"].Value] = point.Report;
                //  }

                //  reportBuilder.AddTable(table);

                //  reportBuilder.AddVerticalSpace();
                //  reportBuilder.AddVerticalSpace();
                //}

                //reportBuilder.AddSignature(managerSignature, Align.Right);

                return reportBuilder.GetReport();
            }

            private Task<CompletedErrand> GetErrand(Guid errandId, CancellationToken cancellationToken)
            {
                return db
                    .CompletedErrands
                    .Include(e => e.PointReviewHistory)
                    .ThenInclude(p => p.Records).FirstOrDefaultAsync(e => e.Id == errandId, cancellationToken: cancellationToken);
            }
        }
    }
}
