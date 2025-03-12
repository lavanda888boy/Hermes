using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MongoDB.Driver;

namespace NotificationPreferencesService.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;

            if (exception is FirebaseMessagingException)
            {
                context.Result = new BadRequestObjectResult("Firebase Messaging Error: " + exception.Message);
            }
            else if (exception is MongoWriteException || exception is MongoQueryException 
                    || exception is MongoCommandException)
            {
                context.Result = new BadRequestObjectResult("MongoDB Error: " + exception.Message);
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
