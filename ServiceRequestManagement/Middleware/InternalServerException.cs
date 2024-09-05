namespace PegasusOrderEntryManagement.Utils.Middleware;

public class InternalServerException : Exception
{
    public InternalServerException(string message) : base(message) { }
}
