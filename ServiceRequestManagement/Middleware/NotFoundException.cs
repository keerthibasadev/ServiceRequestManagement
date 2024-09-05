namespace ServiceRequestManagement.Utils.Middleware;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}
