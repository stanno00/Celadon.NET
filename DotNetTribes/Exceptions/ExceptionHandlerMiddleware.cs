using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DotNetTribes.Exceptions
{
    
    /**
     *This middleware runs asynchronously - it starts running during startup, then when it encounters
     * the await statement, stops so as to save resources. In our case it starts and waits for an exception to be thrown.
     * When this happens, the exception enters the switch below, where individual action is taken based on its type.
     * Ultimately, it returns a custom message JSON along with the desired status code. 
     */
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (exception)
                {
                    // Add your own exception here, like so.
                    case SampleException e:
                        //Replace the BadRequest with your own desired status.
                        response.StatusCode = (int) HttpStatusCode.BadRequest;
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int) HttpStatusCode.InternalServerError;
                        break;
                }
                // The message is created in your custom Exception class. See SampleException class for details.
                var result = JsonSerializer.Serialize(new ErrorJSON(exception.Message));
                await response.WriteAsync(result);
            }
        }
    }
}