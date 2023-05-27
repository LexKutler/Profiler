using ProfilerIntegration.System;
using Serilog;

namespace ProfilerWebAPI.Middleware;

/// <summary>
/// Simple global exception handler. Logs errors and unifies server error responses to <see cref="ErrorModel"/>
/// </summary>
public class GlobalErrorHandlerMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            Log.Error($"!--- {ex.Message} ---! " +
                      $"{Environment.NewLine} {Environment.NewLine} " +
                      $"{ex.StackTrace} " +
                      $"{Environment.NewLine} {Environment.NewLine}");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await context.Response.WriteAsync(new ErrorModel()
            {
                StatusCode = context.Response.StatusCode,
                Message = ex.Message
            }.ToString());
        }
    }
}