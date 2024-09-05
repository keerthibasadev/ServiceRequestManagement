using System.Net;

namespace ServiceRequestManagement.Models;

public class BaseErrorResponse
{
    public BaseErrorResponse()
    {
        this.errorMessage = string.Empty;
        this.errorCode = (int)HttpStatusCode.UnprocessableEntity;
    }
    public string errorMessage { get; set; }
    public int errorCode { get; set; }
}
