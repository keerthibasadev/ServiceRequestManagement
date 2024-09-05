using System.Data;

namespace ServiceRequestManagement.DAL.Interface;
public interface IDapperContext
{
    IDbConnection CreateServiceConnection();
}
