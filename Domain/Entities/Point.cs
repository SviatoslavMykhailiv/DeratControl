using Domain.Common;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities
{
    public class Point : AuditableEntity
    {
        public const int RADIUS = 10;

        private int order;
        private Trap trap;

        private readonly HashSet<PointReview> reviews = new HashSet<PointReview>();

        public Guid PerimeterId { get; init; }
        public Guid TrapId { get; set; }
        public Guid SupplementId { get; set; }

        public int Order
        {
            get => order;
            set
            {
                if (value <= 0)
                    throw new InvalidOperationException("Order cannot be less than 0.");

                var pointWithSameOrderExists = Perimeter.Points.Any(p => p.Order == value && p.Trap == trap && p != this);

                if (pointWithSameOrderExists)
                    throw new InvalidOperationException($"Point with order {value} already exists.");

                order = value;
            }
        }

        public int LeftLoc { get; set; }
        public int TopLoc { get; set; }

        public Trap Trap
        {
            get => trap;
            set
            {
                var pointWithSameTrapExists = Perimeter.Points.Any(p => p.Order == order && p.Trap == value && p != this);

                if (pointWithSameTrapExists)
                    throw new InvalidOperationException($"Point with trap {value} already exists.");

                trap = value;
            }
        }

        public Supplement Supplement { get; set; }
        public Perimeter Perimeter { get; init; }

        public IEnumerable<PointReview> Reviews => reviews;

        public QRID GetIdentifier() => new QRID(PerimeterId, Order, TrapId);
    }
}
