namespace ServiceRequestManagement.DAL;

/// <summary>
/// Used to declare database constants values
/// </summary>
public static class DAConstants
{
    public const string KEY_DB_CONNECTION = "ASPNETCORE_PEGASUSORDERENTRYDBCONNECTION";
    public const string KEY_ENVIRONMENT = "EnvironmentTarget";

    public const string PARAM_MODE = "@_Mode";
    public const int PARAM_MODE_ONE = 1;
    public const int PARAM_MODE_TWO = 2;
    public const int PARAM_MODE_THREE = 3;

    public static class DAServiceRequests
    {
        public const string PARAM_REQUESTID= "@RequestId";
        public const string PARAM_BUILDINGCODE = "@BuildingCode";
        public const string PARAM_DESCRIPTION = "@Description";
        public const string PARAM_CURRENTSTATUS = "@CurrentStatus";
        public const string PARAM_CREATEDBY = "@CreatedBy";
        public const string PARAM_CREATEDDATE = "@CreatedDate";
        public const string PARAM_LASTMODIFIEDBY = "@ModifiedBy";
        public const string PARAM_LASTMODIFIEDDATE = "@ModifiedDate";
        public const string SN_RETRIEVE_REQUESTS = "[dbo].[sp_getServiceRequests]";
        public const string SN_EXECUTE_REQUESTS = "[dbo].[sp_updServiceRequest]";
        public const string SN_DELETE_REQUESTS = "[dbo].[sp_delServiceRequest]";
    }
    public static class DALogs
    {
        public const string PARAM_LOGTYPE = "@LogType";
        public const string PARAM_SESSIONID = "@SessionId";
        public const string PARAM_IPADDRESS = "@IPAddress";
        public const string PARAM_ERRORORIGIN = "@ErrorOrigin";
        public const string PARAM_ERRORCODE = "@ErrorCode";
        public const string PARAM_EXCEPTION = "@Exception";
        public const string PARAM_STACKTRACE = "@StackTrace";
        public const string PARAM_QUERYSTRING = "@QueryString";
        public const string PARAM_HEADERS = "@Headers";
        public const string PARAM_REQUESTDATA = "@RequestData";
        public const string PARAM_RESPONSEDATA = "@ResponseData";

        public const string PARAM_REQUESTID = "@RequestId";
        public const string PARAM_ACCOUNTNO = "@AccountNo";
        public const string PARAM_USERNAME = "@UserName";

        public const string SN_EXECUTE_SERVICEREQUESTLOGS = "[dbo].[sp_intServiceRequestLogs]";
    }
}