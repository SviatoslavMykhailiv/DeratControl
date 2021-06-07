using System;
using System.Linq;
using System.Text;
using Application.Common.Interfaces;
using DinkToPdf;
using DinkToPdf.Contracts;

namespace DeratControl.Infrastructure.Services.Reports
{
    internal class HTMLReportBuilder : IReportBuilder
    {
        private readonly IConverter converter;
        private readonly StringBuilder contentBuilder = new StringBuilder();

        private string content;

        private string reportTitle;

        public HTMLReportBuilder(IConverter converter)
        {
            this.converter = converter;

            contentBuilder.Append($@"<!DOCTYPE html>
                                <html>
                                <head></head>
                                <body>");
        }

        public IReportBuilder AddCheckbox(bool isChecked, string caption)
        {
            var isCheckedStr = isChecked ? "checked" : string.Empty;

            contentBuilder.Append($@"<div><label><input {isCheckedStr} type=""checkbox"">{caption}</label></div>");

            return this;
        }

        public IReportBuilder AddTable(Table table)
        {
            contentBuilder.Append(@"<table style=""width:100%; border-spacing:5px;border: 1px solid black;border-collapse: collapse;"">");

            var headRows = string.Concat(table.Columns.Select(c => $@"<th style=""border: 1px solid black;border-collapse: collapse;padding: 5px;text-align: left;"">{c.Name}</th>"));
            contentBuilder.Append($"<tr>{headRows}</tr>");

            foreach (var row in table)
            {
                contentBuilder.Append("<tr>");

                foreach (var column in table.Columns)
                {
                    var value = (row[column]) switch
                    {
                        "True" => @"<input type=""checkbox"" checked/>",
                        "False" => string.Empty,
                        _ => row[column]
                    };

                    contentBuilder.Append($@"<td style=""padding: 5px;border: 1px solid black;border-collapse: collapse; background-color:{column.Color}"">{value}</td>");
                }
                    

                contentBuilder.Append("</tr>");
            }

            contentBuilder.Append("</table>");
            return this;
        }

        public IReportBuilder AddText(string content, Align align, uint fontSize)
        {
            contentBuilder.Append($@"<div style=""font-size:{fontSize}px; text-align:{GetTextAlign(align)}"">{content}</div>");
            return this;
        }

        public IReportBuilder AddTitle(string title)
        {
            reportTitle = title;
            return this;
        }

        public IReportBuilder AddVerticalSpace()
        {
            contentBuilder.Append(@"<br/>");
            return this;
        }

        private void AddEnding()
        {
            contentBuilder.Append(@"</body></html>");
        }

        private static string GetTextAlign(Align textAlign)
        {
            return textAlign switch
            {
                Align.Left => "left",
                Align.Center => "center",
                Align.Right => "right",
                _ => "left"
            };
        }

        private void FinalizeContent()
        {
            if (content != null)
                return;

            AddEnding();
            content = contentBuilder.ToString();
        }

        public byte[] GetReport()
        {
            FinalizeContent();

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = reportTitle
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = content,
                WebSettings = { DefaultEncoding = "utf-8" },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = string.Empty, Line = false },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = reportTitle }
            };

            var pdf = new HtmlToPdfDocument() { GlobalSettings = globalSettings, Objects = { objectSettings } };

            return converter.Convert(pdf);
        }

    public IReportBuilder AddSignature(byte[] signature, Align align) {

      contentBuilder.Append($"<div text-align:{GetTextAlign(align)}>");
      contentBuilder.Append(GetImgTag(signature));
      contentBuilder.Append("</div>");

      return this;
    }

    private static string GetImgTag(byte[] image) {
      var base64 = Convert.ToBase64String(image);
      return $@"<img height=""60"" width=""60"" src=""data:image/png;base64,{base64}""/>";
    }
  }
}