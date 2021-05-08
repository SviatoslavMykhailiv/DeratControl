using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Resources;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using System.Linq;
using System.Collections.Generic;

namespace Application.Errands.Commands.GenerateReport {
  public record GenerateReportCommand : IRequest<byte[]> {
    public GenerateReportCommand(Guid errandId) {
      ErrandId = errandId;
    }

    public Guid ErrandId { get; }

    public class GenerateReportCommandHandler : IRequestHandler<GenerateReportCommand, byte[]> {
      private readonly IDeratControlDbContext db;
      private readonly IReportBuilder reportBuilder;
      private readonly IStringLocalizer<SharedResource> localizer;

      public GenerateReportCommandHandler(
        IDeratControlDbContext db, 
        IReportBuilder reportBuilder, 
        IStringLocalizer<SharedResource> localizer) {
        this.db = db;
        this.reportBuilder = reportBuilder;
        this.localizer = localizer;
      }

      public async Task<byte[]> Handle(GenerateReportCommand request, CancellationToken cancellationToken) {
        var errand = await GetErrand(request.ErrandId, cancellationToken) ?? throw new NotFoundException();

        //var providerName = errand.Provider.Company == null ? errand.Provider.Name.GetFullName(CultureInfo.CurrentCulture) : errand.Provider.Company.CompanyName;

        reportBuilder
            .AddVerticalSpace()
            .AddText("ТОВ Залупа", Align.Center, 32)
            .AddVerticalSpace()
            .AddText($"{localizer["ReviewDate"]}: {errand.CompleteDate.Value.ToString("dd/MM/yyyy", CultureInfo.CurrentCulture)}", Align.Right, 20)
            .AddVerticalSpace()
            .AddText($"{localizer["ReviewedBy"]}: {errand.Employee.GetFullName()}", Align.Right, 20)
            .AddVerticalSpace()
            .AddText(errand.Description, Align.Center, 24)
            .AddVerticalSpace()
            .AddText(errand.Facility.CompanyName, Align.Center, 24)
            .AddVerticalSpace();

        var points = errand.Points.GroupBy(t => t.Point.Trap);

        var genericColumns = new List<string> { localizer["TrapOrder"], localizer["SupplementName"], localizer["Action"] };

        foreach (var trap in points) {
          var tableColumns = new List<string>(genericColumns);
          tableColumns.AddRange(trap.Key.Fields.OrderBy(f => f.Order).Select(field => field.FieldName));

          tableColumns.Add(localizer["Notes"]);

          var table = new Table(tableColumns);

          foreach (var point in trap) {
            var row = table.NewRow();

            row[genericColumns[0]] = point.Point.Order.ToString();
            row[genericColumns[1]] = point.Point.Supplement.SupplementName;
            row[genericColumns[2]] = point.Status.ToString();

            foreach (var field in trap.Key.Fields.OrderBy(f => f.Order))
              row[field.FieldName] = point[field.Id];

            row[localizer["Notes"]] = point.Report;
          }

          reportBuilder.AddTable(table);

          reportBuilder.AddVerticalSpace();
          reportBuilder.AddVerticalSpace();
        }

        return reportBuilder.GetReport();

      }

      private async Task<Errand> GetErrand(Guid errandId, CancellationToken cancellationToken) {
        return await db
          .Errands
          .Include(e => e.Facility)
          .Include(e => e.Employee)
          .Include(e => e.Points)
          .ThenInclude(p => p.Point)
          .ThenInclude(p => p.Supplement)
          .Include(e => e.Points)
          .ThenInclude(p => p.Records)
          .ThenInclude(r => r.Field)
          .ThenInclude(f => f.Trap)
          .FirstOrDefaultAsync(e => e.Id == errandId, cancellationToken);
      }
    }
  }
}
