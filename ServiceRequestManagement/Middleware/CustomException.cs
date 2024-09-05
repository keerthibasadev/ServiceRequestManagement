namespace ServiceRequestManagement.Utils.Middleware;

public class CustomException : Exception
{
    public CustomException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
