namespace ServiceRequestManagement.Models;

public class BaseSuccessResponse
{
    public BaseSuccessResponse(string _message)
    {
        message = _message;
    }
    public string message { get; set; }
}
