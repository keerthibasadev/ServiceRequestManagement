namespace ServiceRequestManagement.Models;

public class BaseErrorDetails
{
    public BaseErrorDetails()
    {
        this.sessionId = string.Empty;
        this.ipAddress = string.Empty;
        this.errorOrigin = string.Empty;
        this.errorCode = 0;
        this.logType = 0;
    }

    public int? logType { get; set; }
    public string? errorMessage { get; set; }
    public string? sessionId { get; set; }
    public string? ipAddress { get; set; }
    public string? errorOrigin { get; set; }
    public int errorCode { get; set; }
    public DateTime? errorDate
    {
        get
        {
            if (!string.IsNullOrEmpty(this.errorMessage))
                return DateTime.Now;
            else
                return null;
        }
    }
    public string? errorType { get; set; }
    public string? stackTrace { get; set; }
    public string? headers { get; set; }
    public string? requestData { get; set; }
    public string? responseData { get; set; }
    public string? queryString { get; set; }
    public string? accountNo { get; set; }
    public string? requestId { get; set; }
    public string? userName { get; set; }
}
