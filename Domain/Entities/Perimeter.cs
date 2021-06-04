using Domain.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Domain.Entities
{
    public class Perimeter : AuditableEntity
    {
        private readonly HashSet<Point> points = new HashSet<Point>();

        public Guid FacilityId { get; init; }

        public int LeftLoc { get; set; }
        public int TopLoc { get; set; }
        public string PerimeterName { get; set; }
        public Facility Facility { get; init; }
        public IEnumerable<Point> Points => points;
        public decimal Scale { get; set; }

        public string SchemeImagePath { get; private set; }

        public void SetPoint(Guid? pointId, int order, int leftLoc, int topLoc, Trap trap, Supplement supplement)
        {
            if (pointId.HasValue)
                UpdatePoint(pointId.Value, order, leftLoc, topLoc, trap, supplement);
            else
                AddPoint(order, leftLoc, topLoc, trap, supplement);
        }

        public void RemovePoint(Guid pointId)
        {
            points.RemoveWhere(p => p.Id == pointId);
        }

        public void GenerateSchemePath(string format)
        {
            SchemeImagePath = Path.Combine("perimeters", "schemes", $"{Id}.{format}");
        }

        private void AddPoint(int order, int leftLoc, int topLoc, Trap trap, Supplement supplement)
        {
            if (points.Any(p => p.Order == order && p.Trap == trap && p.Supplement == supplement))
                throw new InvalidOperationException($"Point with order {order} already exists.");

            var point = new Point
            {
                Perimeter = this,
                Order = order,
                LeftLoc = leftLoc,
                TopLoc = topLoc,
                Trap = trap,
                Supplement = supplement
            };

            points.Add(point);
        }

        private void UpdatePoint(Guid pointId, int order, int leftLoc, int topLoc, Trap trap, Supplement supplement)
        {
            if (points.Any(p => p.Order == order && p.Trap == trap && p.Supplement == supplement && p.Id != pointId))
                throw new InvalidOperationException($"Point with order {order} already exists.");

            var point = points.First(p => p.Id == pointId);

            point.Order = order;
            point.LeftLoc = leftLoc;
            point.TopLoc = topLoc;
            point.Trap = trap;
            point.Supplement = supplement;
        }

        public byte[] GeneratePerimeterImage(byte[] image)
        {
            using var imageStream = new MemoryStream(image);
            using var bitmap = new Bitmap(imageStream);

            var graphics = Graphics.FromImage(bitmap);

            var linePen = new Pen(Brushes.Gray);
            var font = new Font(new FontFamily("Times New Roman"), 20, FontStyle.Regular, GraphicsUnit.Pixel);

            foreach (var point in Points)
            {
                var brush = new SolidBrush(ColorTranslator.FromHtml(point.Trap.Color));
                graphics.FillEllipse(brush, point.LeftLoc - Point.RADIUS / 2, point.TopLoc - Point.RADIUS / 2, Point.RADIUS, Point.RADIUS);
                graphics.DrawString(point.Order.ToString(), font, Brushes.Black, new PointF(point.LeftLoc + Point.RADIUS / 2, point.TopLoc - Point.RADIUS));
            }

            using var memoryStream = new MemoryStream();
            var encoder = ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == ImageFormat.Png.Guid);

            using var encodingParams = new EncoderParameters(1);
            encodingParams.Param[0] = new EncoderParameter(Encoder.Quality, 85L);

            bitmap.Save(memoryStream, encoder, encodingParams);

            return memoryStream.ToArray();
        }
    }
}
