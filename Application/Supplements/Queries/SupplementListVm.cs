using System.Collections.Generic;

namespace Application.Supplements.Queries {
  public class SupplementListVm {
    public IReadOnlyCollection<SupplementDto> Supplements { get; set; }
  }
}
