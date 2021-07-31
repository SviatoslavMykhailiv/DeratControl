using System;
using System.Collections.Generic;

namespace Application.Perimeters.Commands.UpsertPerimeter
{
    public class PointDto
    {
        public Guid? PointId { get; init; }
        public Guid TrapId { get; init; }
        public Guid SupplementId { get; init; }
        public int Order { get; init; }
        public int LeftLoc { get; init; }
        public int TopLoc { get; init; }
        public IReadOnlyCollection<PointFieldValueDTO> Records { get; init; } = new List<PointFieldValueDTO>();
    }
}
