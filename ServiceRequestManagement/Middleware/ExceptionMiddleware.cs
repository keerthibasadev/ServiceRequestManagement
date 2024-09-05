using PegasusOrderEntryManagement.Utils.Middleware;
using ServiceRequestManagement.BAL.Interface;
using ServiceRequestManagement.Models;
using ServiceRequestManagement.Utils.Middleware;
using System.Net;
using System.Text.Json;

namespace ServiceRequestManagement.Middleware;
/// <summary>
/// Middleware to handle exceptions and log errors.
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IBALogManagement _baLogManagement;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, IBALogManagement baLogManagement, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _baLogManagement = baLogManagement;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;

        using var responseBodyStream = new MemoryStream();
        context.Response.Body = responseBodyStream;

        try
        {
            await _next(context);

            // Log response after successful processing
            await LogResponseAsync(context);
        }
        catch (Exception ex)
        {
            // Handle exception and log error
            await HandleExceptionAsync(context, ex);
        }
        finally
        {
            // Copy the response body back to the original stream
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            await responseBodyStream.CopyToAsync(originalBodyStream);
            context.Response.Body = originalBodyStream;
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var baseError = new BaseErrorDetails
        {
            sessionId = context.Connection.Id.ToString(),
            ipAddress = context.Connection.RemoteIpAddress?.ToString(),
            errorOrigin = context.Request.Path,
            errorCode = (int)GetStatusCode(ex),
            errorType = ex.GetType().Name,
            errorMessage = ex.Message,
            stackTrace = ex.StackTrace
        };

        _logger.LogError(ex, "Exception occurred: {ErrorDetails}", baseError);

        try
        {
            await _baLogManagement.SaveGenericLogs(baseError);
        }
        catch (Exception logEx)
        {
            _logger.LogError(logEx, "Failed to log exception details: {ErrorDetails}", baseError);
        }

        var errorResponse = new BaseErrorResponse
        {
            errorMessage = "An error occurred while processing your request.",
            errorCode = baseError.errorCode
        };

        var responseJson = JsonSerializer.Serialize(errorResponse);
        context.Response.Clear();
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)baseError.errorCode;

        await context.Response.WriteAsync(responseJson);
    }
    private static HttpStatusCode GetStatusCode(Exception ex)
    {
        HttpStatusCode statusCode;

        switch (ex)
        {
            case BadRequestException:
                statusCode = HttpStatusCode.BadRequest;
                break;
            case NotFoundException:
            case NoContentFoundException:
                statusCode = HttpStatusCode.NotFound;
                break;
            case InternalServerException:
                statusCode = HttpStatusCode.InternalServerError;
                break;
            default:
                statusCode = HttpStatusCode.InternalServerError;
                break;
        }

        return statusCode;
    }

    private async Task LogResponseAsync(HttpContext context)
    {
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
        context.Response.Body.Seek(0, SeekOrigin.Begin);

        _logger.LogInformation("HTTP Response \nPath: {Path} \nStatus: {StatusCode} - Response: {Body}",
            context.Request.Path, context.Response.StatusCode, responseBody);
    }
}