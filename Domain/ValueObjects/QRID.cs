using System;

namespace Domain.ValueObjects
{
    public readonly struct QRID
    {
        public QRID(Guid facilityId, Guid perimeterId, int order, Guid trapId)
        {
            Oi = facilityId;
            Pi = perimeterId;
            Or = order;
            Ti = trapId;
        }

        public Guid Oi { get; }
        public Guid Pi { get; }
        public int Or { get; }
        public Guid Ti { get; }
    }
}
