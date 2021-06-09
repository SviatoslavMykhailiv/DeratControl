using System;

namespace Domain.ValueObjects
{
    public readonly struct QRID
    {
        public QRID(Guid perimeterId, int order, Guid trapId)
        {
            PerimeterId = perimeterId;
            Order = order;
            TrapId = trapId;
        }

        public Guid PerimeterId { get; }
        public int Order { get; }
        public Guid TrapId { get; }

        public override string ToString()
        {
            return $"{PerimeterId}&{Order}&{TrapId}";
        }

        public static implicit operator string(QRID id) => id.ToString();
    }
}
