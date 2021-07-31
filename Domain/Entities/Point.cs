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
        private Supplement supplement;

        private readonly HashSet<PointReview> reviews = new HashSet<PointReview>();
        private readonly HashSet<PointFieldValue> values = new HashSet<PointFieldValue>();

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

                var pointWithSameOrderExists = Perimeter.Points.Any(p => p.Order == value && p.Trap == trap && p.Supplement == supplement && p != this);

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
                var pointWithSameTrapExists = Perimeter.Points.Any(p => p.Order == order && p.Supplement == supplement && p.Trap == value && p != this);

                if (pointWithSameTrapExists)
                    throw new InvalidOperationException($"Point with trap {value} already exists.");

                if(trap != value)
                {
                    values.Clear();
                }

                trap = value;
            }
        }

        public void SetFieldValue(Guid fieldId, string value) 
        {
            var field = trap.Fields.FirstOrDefault(f => f.Id == fieldId) ?? throw new InvalidOperationException("Поле не знайдено.");

            if (field.AdminEditable == false)
                return;

            var fieldValue = values.FirstOrDefault(v => v.FieldId == fieldId);

            if(fieldValue is null)
            {
                fieldValue = new PointFieldValue { Point = this, Field = field, Value = value };
                values.Add(fieldValue);
            }
            else
            {
                fieldValue.Value = field.AdjustValue(value);
            }
        }

        public Supplement Supplement 
        {
            get => supplement;
            set
            {
                var pointWithSameSupplementExists = Perimeter.Points.Any(p => p.Order == order && p.Supplement == value && p.Trap == trap && p != this);

                if (pointWithSameSupplementExists)
                    throw new InvalidOperationException($"Point with supplement {value} already exists.");

                supplement = value;
            }
        }

        public Perimeter Perimeter { get; init; }

        public IEnumerable<PointReview> Reviews => reviews;

        public IEnumerable<PointFieldValue> Values => values;

        public QRID GetIdentifier() => new QRID(PerimeterId, Order, TrapId);
    }
}
