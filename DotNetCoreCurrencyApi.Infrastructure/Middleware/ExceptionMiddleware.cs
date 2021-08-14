using DotNetCoreCurrencyApi.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DotNetCoreCurrencyApi.Infrastructure.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                string msg = "Something went wrong !Internal Server Error";
                _logger.LogError($"Unhandled error ocurred: {ex.Message}; Host: {httpContext.Request.Host}; Path: {httpContext.Request.Path}");
                await HandleGlobalExceptionAsync(httpContext, ex, msg);
            }
        }

        private static Task HandleGlobalExceptionAsync(HttpContext context, Exception exception, string msg)
        {
            // we can redirect to a web page if we have one
            //context.Response.Redirect($"/home/error/{msg}");

            //context.Response.ContentType = "application/json";
            //context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(new ResponseModel
            {
                StatusCode = context.Response.StatusCode,
                Message = msg
            }.ToString());
        }
    }


}
