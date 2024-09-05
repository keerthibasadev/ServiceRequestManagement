using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PegasusOrderEntryManagement.Utils;
using Serilog;
using ServiceRequestManagement.Models;
using ServiceRequestManagement.Utils.Middleware;
using System.Net;

namespace ServiceRequestManagement.Middleware;

public class BadRequestResultFilter : IResultFilter
{
    public void OnResultExecuted(ResultExecutedContext context)
    {
    }

    /// <summary>
    /// create response for bad request
    /// </summary>
    /// <param name="context">ResultExecutingContext</param>
    public void OnResultExecuting(ResultExecutingContext context)
    {
        try
        {
            if (!context.ModelState.IsValid)
            {
                BaseErrorDetails errorDetails = CreateErrorDetails(context);
                List<string> lstError = new();

                foreach (var modalStatePair in context.ModelState)
                {
                    try
                    {
                        string key = modalStatePair.Key;
                        if (key.Contains('$'))
                        {
                            string actualKey = string.Empty;
                            try
                            {
                                actualKey = ": Invalid " + string.Concat(key[2..].Select((x, i) => i > 0 && char.IsUpper(x) ? " " + x : x.ToString())).ToLower();
                            }
                            catch
                            {
                                actualKey = key.Length > 2 ? ": Invalid " + key[2..] : string.Empty;
                            }
                            lstError = new List<string>
                            {
                                "Bad request" + actualKey
                            };
                            break;
                        }
                        ModelErrorCollection modelErrors = modalStatePair.Value.Errors;
                        if (modelErrors is not null && modelErrors.Count > 0)
                        {
                            List<string> errors = modelErrors.Select(error => error.ErrorMessage).ToList();
                            if (errors != null && errors.Any())
                                foreach (string error in errors)
                                    lstError.AddRange(error.Split(","));
                        }
                    }
                    catch (Exception ex)
                    {
                        LogException(ex, errorDetails.errorType.ToString());
                    }
                }
                errorDetails.errorMessage = string.Join(",", lstError);
                LogErrorDetails(errorDetails.errorType.ToString(), errorDetails.errorMessage);

                BaseErrorResponse errorResponse = new()
                {
                    errorMessage = errorDetails.errorMessage,
                    errorCode = (int)HttpStatusCode.BadRequest
                };
                context.Result = new BadRequestObjectResult(errorResponse);
            }
        }
        catch (Exception ex)
        {
            LogException(ex, "BadRequest");
            throw new CustomException(ex.Message, ex);
        }
    }

    /// <summary>
    /// Create BaseErrorDetails Object with default data
    /// </summary>
    /// <param name="context">ResultExecutingContext</param>
    /// <returns>BaseErrorDetails</returns>
    private static BaseErrorDetails CreateErrorDetails(ResultExecutingContext context)
    {
        return new BaseErrorDetails
        {
            ipAddress = context.HttpContext.Connection.RemoteIpAddress.ToString(),
            sessionId = context.HttpContext.Connection.Id,
            errorCode = (int)HttpStatusCode.BadRequest,
            errorType = "BadRequest",
            errorOrigin = "Client Error"
        };
    }

    /// <summary>
    /// Log Error to file
    /// </summary>
    /// <param name="errorType">string</param>
    /// <param name="errorMessage">string</param>
    private static void LogErrorDetails(string errorType, string errorMessage)
    {
        Log.Information("------------------------Global Attribute Filter Logging Start------------------------");
        Log.Error("Warning Code: {StatusCode}, \r\n Warning Type: {ErrorType}, \r\n Warning: {ErrorMessage}",
                  (int)HttpStatusCode.BadRequest, errorType, errorMessage);
        Log.Information("------------------------Global Attribute Filter Logging End------------------------");
    }

    /// <summary>
    /// Log Exception to file
    /// </summary>
    /// <param name="ex">Exception</param>
    /// <param name="errorType">string</param>
    private static void LogException(Exception ex, string errorType)
    {
        Log.Information(ex, "------------------------Global Attribute Filter Logging Start------------------------");
        Log.Error("Exception Code: {StatusCode}, \r\n Exception Type: {ErrorType}, \r\n Exception: {Message}, \r\n StackTrace: {StackTrace}",
                  (int)HttpStatusCode.InternalServerError, errorType, ex.Message, ex.StackTrace);
        Log.Information("------------------------Global Attribute Filter Logging End------------------------");
    }
}