using System;

namespace Domain.ValueObjects
{
    public readonly struct QRID
    {
        public QRID(Guid facilityId, Guid perimeterId, int order, Guid trapId)
        {
            FacilityId = facilityId;
            PerimeterId = perimeterId;
            Order = order;
            TrapId = trapId;
        }

        public Guid FacilityId { get; }
        public Guid PerimeterId { get; }
        public int Order { get; }
        public Guid TrapId { get; }

        public override string ToString()
        {
            return $"{FacilityId}&{PerimeterId}&{Order}&{TrapId}";
        }

        public static implicit operator string(QRID id) => id.ToString();
    }
}
