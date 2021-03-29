using System.Collections.Generic;

namespace Application.Facilities.Queries.GetFacilityList {
  public class FacilityListVm {
    public ICollection<FacilityHeaderDto> Facilities { get; set; }
  }
}
