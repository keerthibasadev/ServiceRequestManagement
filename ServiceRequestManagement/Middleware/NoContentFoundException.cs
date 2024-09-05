namespace ServiceRequestManagement.Utils.Middleware;

public class NoContentFoundException : Exception
{
    public NoContentFoundException(string message) : base(message) { }
}
