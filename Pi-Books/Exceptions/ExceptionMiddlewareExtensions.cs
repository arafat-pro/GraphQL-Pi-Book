using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Pi_Books.Data.ViewModels;

namespace Pi_Books.Exceptions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureBuiltInExceptionHandler(this IApplicationBuilder appBuilder, ILoggerFactory loggerFactory)
        {
            appBuilder.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    var logger = loggerFactory.CreateLogger("ConfigureBuiltInExceptionHandler");

                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var contextRequest = context.Features.Get<IHttpRequestFeature>();

                    if (contextFeature != null)
                    {
                        var errorVMString = new ErrorVM()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message,
                            Path = contextRequest.Path
                        }.ToString();

                        logger.LogError(errorVMString);
                        await context.Response.WriteAsync(errorVMString);
                    }
                });
            });
        }

        public static void ConfigureCustomExceptionHandler(this IApplicationBuilder appBuilder)
        {
            appBuilder.UseMiddleware<CustomExceptionMiddleware>();
        }
    }
}