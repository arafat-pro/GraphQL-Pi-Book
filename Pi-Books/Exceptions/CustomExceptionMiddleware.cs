using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Pi_Books.Data.ViewModels;

namespace Pi_Books.Exceptions
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate requestDelegate;

        public CustomExceptionMiddleware(RequestDelegate requestDelegate)
        {
            this.requestDelegate = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await requestDelegate(httpContext);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(httpContext, exception);
            }
        }

        private Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            httpContext.Response.ContentType = "application/json";

            var response = new ErrorVM()
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = "Internal Server Error from the Custom Middle-ware!",
                Path = httpContext.Request.Path
            };

            return httpContext.Response.WriteAsync(response.ToString());
        }
    }
}