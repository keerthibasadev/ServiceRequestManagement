using ServiceRequestManagement.Models;
using System.Net;

namespace ServiceRequestManagement.Common;

public static class ErrorResponseUtility
{
     public static BaseErrorResponse BadRequestResponse(string? errorMessage = null)
    {
        BaseErrorResponse errorResponse = new()
        {
            errorCode = (int)HttpStatusCode.BadRequest,
            errorMessage = !string.IsNullOrEmpty(errorMessage) ? errorMessage : AppMessages.INVALID_INPUTS
        };
        return errorResponse;
    }

    public static BaseErrorResponse NotFoundResponse(string errorMessage)
    {
        BaseErrorResponse errorResponse = new()
        {
            errorCode = (int)HttpStatusCode.NotFound,
            errorMessage = errorMessage
        };
        return errorResponse;
    }

    public static BaseErrorResponse UnprocessableEntityResponse(string errorMessage)
    {
        BaseErrorResponse errorResponse = new()
        {
            errorCode = (int)HttpStatusCode.UnprocessableEntity,
            errorMessage = errorMessage
        };
        return errorResponse;
    }
}
