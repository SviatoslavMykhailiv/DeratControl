using Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filters {
  public class ApiExceptionFilter : ExceptionFilterAttribute {
    public override void OnException(ExceptionContext context) {
      if(context.Exception is BadRequestException badRequestEx)  {
        context.Result = new BadRequestObjectResult(badRequestEx);
        return;
      }

      if (context.Exception is NotFoundException notFoundEx) {
        context.Result = new NotFoundObjectResult(notFoundEx);
        return;
      }
    }
  }
}
