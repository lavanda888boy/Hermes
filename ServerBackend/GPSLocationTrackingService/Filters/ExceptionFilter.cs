using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace GPSLocationTrackingService.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;

            if (exception is RedisServerException)
            {
                context.Result = new BadRequestObjectResult("Redis Request Error: " + exception.Message);
            }
            else
            {
                context.Result = new ObjectResult(exception.Message)
                {
                    StatusCode = 500
                };
            }

            context.ExceptionHandled = true;
        }
    }
}
