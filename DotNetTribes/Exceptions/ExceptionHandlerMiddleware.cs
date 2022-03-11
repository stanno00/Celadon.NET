using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace DotNetTribes.Exceptions
{
    public class ExceptionHandlerMiddleware
    {
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
                        case SampleException e:
                            response.StatusCode = (int) HttpStatusCode.BadRequest;
                            break;
                        case LoginException e:
                            response.StatusCode = (int) HttpStatusCode.BadRequest;
                            break;
                        default:
                            response.StatusCode = (int) HttpStatusCode.InternalServerError;
                            break;
                    }
                    var result = JsonSerializer.Serialize(new ErrorJSON(exception.Message));
                    await response.WriteAsync(result);
                }
            }
        }
    }
}