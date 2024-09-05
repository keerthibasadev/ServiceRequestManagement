using ServiceRequestManagement.Models;

namespace ServiceRequestManagement.BAL.Interface;

public interface IBALogManagement
{
    Task<int> SaveGenericLogs(BaseErrorDetails errorDetails);
}
