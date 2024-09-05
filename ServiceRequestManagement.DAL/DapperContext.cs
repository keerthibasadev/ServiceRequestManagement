using ServiceRequestManagement.DAL.Interface;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
namespace ServiceRequestManagement.DAL;

/// <summary>
/// Get the Connection string from appsettings.json and creact sql connection object
/// <returns>Return Sql Connection</returns>
/// </summary>
public class DapperContext : IDapperContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    /// <summary>
    /// constroctor to initialize IConfiguration object
    /// </summary>
    /// <param name="configuration">IConfiguration</param>
    public DapperContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = configuration.GetConnectionString("ServiceRequestConnection");
    }

    /// <summary>
    /// Getting Service request db connection object
    /// </summary>
    /// <returns>IDbConnection</returns>
    public IDbConnection CreateServiceConnection() =>
        new SqlConnection(_connectionString);

}
