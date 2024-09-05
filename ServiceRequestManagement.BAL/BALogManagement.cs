using Dapper;
using ServiceRequestManagement.BAL.Interface;
using ServiceRequestManagement.DAL;
using ServiceRequestManagement.DAL.Interface;
using ServiceRequestManagement.Models;

namespace ServiceRequestManagement.BAL;

public class BALogManagement : IBALogManagement
{
    #region Constructors

    private readonly IRepository _repository;

    /// <summary>
    /// constructor to initialize Repository
    /// </summary>
    /// <param name="context"></param>
    public BALogManagement(IDapperContext context)
    {
        _repository = new Repository(context.CreateServiceConnection());
    }

    #endregion

    /// <summary>
    /// Use to save the logs
    /// </summary>
    /// <param name="errorDetails">BaseErrorDetails</param>
    /// <returns>int</returns>
    public async Task<int> SaveGenericLogs(BaseErrorDetails errorDetails)
    {
        DynamicParameters parameters = new();
        parameters.Add(DAConstants.PARAM_MODE, DAConstants.PARAM_MODE_ONE);
        parameters.Add(DAConstants.DALogs.PARAM_LOGTYPE, errorDetails.logType);
        parameters.Add(DAConstants.DALogs.PARAM_SESSIONID, errorDetails.sessionId);
        parameters.Add(DAConstants.DALogs.PARAM_IPADDRESS, errorDetails.ipAddress);
        parameters.Add(DAConstants.DALogs.PARAM_ERRORORIGIN, errorDetails.errorOrigin);
        parameters.Add(DAConstants.DALogs.PARAM_ERRORCODE, errorDetails.errorCode);
        parameters.Add(DAConstants.DALogs.PARAM_EXCEPTION, errorDetails.errorCode == 0 ? null : errorDetails.errorType + "# " + errorDetails.errorMessage);
        parameters.Add(DAConstants.DALogs.PARAM_STACKTRACE, errorDetails.stackTrace);
        parameters.Add(DAConstants.DALogs.PARAM_QUERYSTRING, errorDetails.queryString);
        parameters.Add(DAConstants.DALogs.PARAM_HEADERS, errorDetails.headers);
        parameters.Add(DAConstants.DALogs.PARAM_REQUESTDATA, errorDetails.requestData);
        parameters.Add(DAConstants.DALogs.PARAM_RESPONSEDATA, errorDetails.responseData);
        parameters.Add(DAConstants.DALogs.PARAM_REQUESTID, errorDetails.requestId);
        parameters.Add(DAConstants.DALogs.PARAM_ACCOUNTNO, errorDetails.accountNo);
        parameters.Add(DAConstants.DALogs.PARAM_USERNAME, errorDetails.userName);
        return await _repository.InsertRecord(parameters, DAConstants.DALogs.SN_EXECUTE_SERVICEREQUESTLOGS);
    }
}
