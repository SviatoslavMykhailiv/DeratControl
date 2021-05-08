﻿using Application.Common.Interfaces;
using Application.Resources;
using DinkToPdf;
using DinkToPdf.Contracts;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Services {
  public class QRListGenerator : IQRListGenerator {
    private readonly IQRCodeService qrCodeService;
    private readonly IStringLocalizer<SharedResource> localizer;
    private readonly IEncryptionService encryptionService;
    private readonly IConverter converter;

    public QRListGenerator(
      IQRCodeService qrCodeService,
      IStringLocalizer<SharedResource> localizer,
      IEncryptionService encryptionService,
      IConverter converter) {
      this.qrCodeService = qrCodeService;
      this.localizer = localizer;
      this.encryptionService = encryptionService;
      this.converter = converter;
    }

    public byte[] Generate(IEnumerable<QRID> qrIdList, Facility facility, Dictionary<Guid, Trap> traps) {
      var globalSettings = new GlobalSettings {
        ColorMode = ColorMode.Color,
        Orientation = Orientation.Portrait,
        PaperSize = PaperKind.A4,
        Margins = new MarginSettings { Top = 10 },
        DocumentTitle = $"QR {localizer["Codes"]} - {facility.Name}"
      };

      var pdf = new HtmlToPdfDocument() { GlobalSettings = globalSettings };

      foreach (var identifier in qrIdList.OrderBy(i => i.PerimeterId).GroupBy(i => i.PerimeterId)) {
        var sheetBuilder = new StringBuilder();
        sheetBuilder.Append(@"<!DOCTYPE html><html><head><meta charset=""utf-8""/></head><body>");
        sheetBuilder.Append($@"<div style=""text-align:center; font-size:28px;font-weight:bold;"">{facility.Name}<hr/></div>");
        sheetBuilder.Append($@"<div style=""text-align:center; font-size:20px;"">{localizer["Perimeter"]} - {facility.GetPerimeter(identifier.Key).PerimeterName}</div><br/>");

        foreach (var code in identifier) {
          var imgTag = $@"
                            <div style=""display:inline-block; margin-right:5px; margin-left:5px;"">
                              <div style=""text-align:center; margin-bottom:2px;"">{localizer["Trap"]} №{code.Order}, {traps[code.TrapId].TrapName}</div>
                              {GetImgTag(qrCodeService.Generate(encryptionService.Encrypt(code)))}
                            </div>";

          sheetBuilder.Append(imgTag);
        }

        var objectSettings = new ObjectSettings {
          PagesCount = true,
          HtmlContent = sheetBuilder.ToString(),
          WebSettings = { DefaultEncoding = "utf-8" },
          HeaderSettings = { FontName = "Arial", FontSize = 9, Right = string.Empty, Line = false },
          FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = $"QR {localizer["Codes"]} - {facility.Name}" }
        };

        pdf.Objects.Add(objectSettings);
      }

      return converter.Convert(pdf);
    }

    private static string GetImgTag(byte[] qrCode) {
      var base64 = Convert.ToBase64String(qrCode);
      return $@"<img height=""260"" width=""260"" src=""data:image/png;base64,{base64}""/>";
    }

    public byte[] Generate(Perimeter perimeter, IEnumerable<Guid> pointIdList) {

      var globalSettings = new GlobalSettings {
        ColorMode = ColorMode.Color,
        Orientation = Orientation.Portrait,
        PaperSize = PaperKind.A4,
        Margins = new MarginSettings { Top = 10 },
        DocumentTitle = $"QR {localizer["Codes"]} - {perimeter.Facility.Name}"
      };

      var pdf = new HtmlToPdfDocument() { GlobalSettings = globalSettings };

      var points = perimeter.Points.Where(p => pointIdList.Contains(p.Id));

      var sheetBuilder = new StringBuilder();
      sheetBuilder.Append(@"<!DOCTYPE html><html><head><meta charset=""utf-8""/></head><body>");
      sheetBuilder.Append($@"<div style=""text-align:center; font-size:28px;font-weight:bold;"">{perimeter.Facility.Name}<hr/></div>");
      sheetBuilder.Append($@"<div style=""text-align:center; font-size:20px;"">{localizer["Perimeter"]} - {perimeter.PerimeterName}</div><br/>");

      foreach (var point in points) {
        var imgTag = $@"
                            <div style=""display:inline-block; margin-right:5px; margin-left:5px;"">
                              <div style=""text-align:center; margin-bottom:2px;"">{localizer["Trap"]} №{point.Order}, {point.Trap.TrapName}</div>
                              {GetImgTag(qrCodeService.Generate(encryptionService.Encrypt(point.GetIdentifier())))}
                            </div>";

        sheetBuilder.Append(imgTag);
      }

      var objectSettings = new ObjectSettings {
        PagesCount = true,
        HtmlContent = sheetBuilder.ToString(),
        WebSettings = { DefaultEncoding = "utf-8" },
        HeaderSettings = { FontName = "Arial", FontSize = 9, Right = string.Empty, Line = false },
        FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = $"QR {localizer["Codes"]} - {perimeter.Facility.Name}" }
      };

      pdf.Objects.Add(objectSettings);

      return converter.Convert(pdf); ;
    }
  }
}
