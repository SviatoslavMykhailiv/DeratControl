using System.Collections.Generic;

namespace Application.Traps.Queries.GetTrapList {
  public class TrapListVm {
    public ICollection<TrapDto> Traps { get; set; }
  }
}
