using Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace API.Filters
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<ApiExceptionFilter>>();
            logger.LogError(1, context.Exception, context.Exception.Message);

            if (context.Exception is BadRequestException badRequestEx)
            {
                context.Result = new BadRequestObjectResult(badRequestEx);
                return;
            }

            if (context.Exception is NotFoundException notFoundEx)
            {
                context.Result = new NotFoundObjectResult(notFoundEx);
                return;
            }
        }
    }
}
