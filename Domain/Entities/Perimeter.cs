using Domain.Common;
using ImageMagick;
using ImageMagick.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        public void SetPoint(Guid? pointId, int order, int leftLoc, int topLoc, Trap trap, Supplement supplement, Dictionary<Guid, string> values)
        {
            Point point;

            if (pointId.HasValue)
                point = UpdatePoint(pointId.Value, order, leftLoc, topLoc, trap, supplement);
            else
                point = AddPoint(order, leftLoc, topLoc, trap, supplement);

            foreach (var value in values)
                point.SetFieldValue(value.Key, value.Value);
        }

        public void RemovePoint(Guid pointId)
        {
            points.RemoveWhere(p => p.Id == pointId);
        }

        public void GenerateSchemePath(string format)
        {
            SchemeImagePath = Path.Combine("perimeters", "schemes", $"{Id}.{format}");
        }

        private Point AddPoint(
            int order, 
            int leftLoc, 
            int topLoc, 
            Trap trap, 
            Supplement supplement)
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

            return point;
        }

        private Point UpdatePoint(
            Guid pointId, 
            int order, 
            int leftLoc, 
            int topLoc, 
            Trap trap, 
            Supplement supplement)
        {
            if (points.Any(p => p.Order == order && p.Trap == trap && p.Supplement == supplement && p.Id != pointId))
                throw new InvalidOperationException($"Point with order {order} already exists.");

            var point = points.First(p => p.Id == pointId);

            point.Order = order;
            point.LeftLoc = leftLoc;
            point.TopLoc = topLoc;
            point.Trap = trap;
            point.Supplement = supplement;

            return point;
        }

        public byte[] GeneratePerimeterImage(byte[] image)
        {
            MagickImage magickImage;

            if (image.Length == 0)
            {
                // Create a blank white image 1000x1000
                magickImage = new MagickImage(MagickColors.White, 1000, 1000);
            }
            else
            {
                magickImage = new MagickImage(image);
            }

            var drawables = new Drawables();
            var fontSize = 20;
            var fontFamily = "Times New Roman";

            foreach (var point in Points)
            {
                var color = new MagickColor(point.Trap.Color); // "#RRGGBB"
                var radius = Point.RADIUS;
                var x = point.LeftLoc;
                var y = point.TopLoc;

                // Draw filled circle
                drawables.FillColor(color)
                         .StrokeColor(MagickColors.Transparent)
                         .Circle(x, y, x + radius / 2.0, y);

                // Draw order text
                drawables.Font(fontFamily)
                         .FontPointSize(fontSize)
                         .FillColor(MagickColors.Black)
                         .TextAlignment(TextAlignment.Left)
                         .Text(x + radius / 2.0, y - radius, point.Order.ToString());
            }

            drawables.Draw(magickImage);

            // Set format and quality (PNG doesn't use quality, but for consistency we mimic it)
            magickImage.Format = MagickFormat.Png;

            return magickImage.ToByteArray();
        }
    }
}
