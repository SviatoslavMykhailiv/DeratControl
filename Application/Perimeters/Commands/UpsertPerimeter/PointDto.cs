using System;

namespace Application.Perimeters.Commands.UpsertPerimeter {
  public class PointDto {
    public Guid? PointId { get; set; }
    public Guid TrapId { get; set; }
    public Guid SupplementId { get; set; }
    public int Order { get; set; }
    public int LeftLoc { get; set; }
    public int TopLoc { get; set; }
  }
}
