using System;

namespace Application.Common.Models {
  public record RequestContext(CurrentUser CurrentUser, DateTime CurrentDateTime);
}
