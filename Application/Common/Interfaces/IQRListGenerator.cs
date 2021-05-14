using Application.Common.Dtos;
using Domain.Entities;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace Application.Common.Interfaces
{
    public interface IQRListGenerator
    {
        byte[] Generate(IEnumerable<QRID> qrIdList, Facility facility, Dictionary<Guid, Trap> traps);
        byte[] Generate(Perimeter perimeter, IEnumerable<PointQRDto> pointIdList, Dictionary<Guid, Trap> traps);
    }
}
