using Dapper;
using ServiceRequestManagement.BAL.Interface;
using ServiceRequestManagement.DAL;
using ServiceRequestManagement.DAL.Interface;
using ServiceRequestManagement.Models;
using System.Reflection.Metadata.Ecma335;

namespace ServiceRequestManagement.BAL;

public class BAServiceRequest : IBAServiceRequest
{
    #region Constructors

    private readonly IRepository _repository;

    /// <summary>
    /// constructor to initialize Repository
    /// </summary>
    /// <param name="context"></param>
    public BAServiceRequest(IDapperContext context)
    {
        _repository = new Repository(context.CreateServiceConnection());
    }


    /// <summary>
    /// Get service request list
    /// </summary>
    /// <returns>List of ServiceRequestDetails</returns>
    public async Task<IEnumerable<ServiceRequestDetails>> GetServiceRequests()
    {
        DynamicParameters parameters = new();
        parameters.Add(DAConstants.PARAM_MODE, DAConstants.PARAM_MODE_ONE);
        return await _repository.GetAllRecords<ServiceRequestDetails>(parameters, DAConstants.DAServiceRequests.SN_RETRIEVE_REQUESTS);
    }

    /// <summary>
    /// Get service request details
    /// </summary>
    /// <returns>ServiceRequestDetails</returns>
    public async Task<ServiceRequestDetails> GetServiceRequestById(Guid serviceRequestId)
    {
        DynamicParameters parameters = new();
        parameters.Add(DAConstants.PARAM_MODE, DAConstants.PARAM_MODE_TWO);
        parameters.Add(DAConstants.DAServiceRequests.PARAM_REQUESTID, serviceRequestId);
        return await _repository.GetRecord<ServiceRequestDetails>(parameters, DAConstants.DAServiceRequests.SN_RETRIEVE_REQUESTS);
    }

    /// <summary>
    /// Add service request Infomation
    /// </summary>
    /// <param name="requestDetails">AddServiceRequest</param>
    /// <returns>int</returns>
    public async Task<int> AddServiceRequest(AddServiceRequest requestDetails)
    {
        DynamicParameters parameters = new();
        parameters.Add(DAConstants.PARAM_MODE, DAConstants.PARAM_MODE_ONE);
        parameters.Add(DAConstants.DAServiceRequests.PARAM_BUILDINGCODE, requestDetails.buildingCode);
        parameters.Add(DAConstants.DAServiceRequests.PARAM_DESCRIPTION, requestDetails.description);
        parameters.Add(DAConstants.DAServiceRequests.PARAM_CURRENTSTATUS, requestDetails.currentStatus);
        parameters.Add(DAConstants.DAServiceRequests.PARAM_CREATEDBY, requestDetails.createdBy);
        parameters.Add(DAConstants.DAServiceRequests.PARAM_LASTMODIFIEDBY, requestDetails.createdBy);

        return await _repository.InsertRecord(parameters, DAConstants.DAServiceRequests.SN_EXECUTE_REQUESTS);
    }

    /// <summary>
    /// Update service request Infomation
    /// </summary>
    /// <param name="requestDetails">UpdateServiceRequest</param>
    /// <returns>int</returns>
    public async Task<int> UpdateServiceRequest(UpdateServiceRequest requestDetails)
    {
        DynamicParameters parameters = new();
        parameters.Add(DAConstants.PARAM_MODE, DAConstants.PARAM_MODE_TWO);

        ServiceRequestDetails serviceRequest = await GetServiceRequestById(requestDetails.serviceRequestId);

        if (serviceRequest != null)
        {
            if (!string.IsNullOrEmpty(requestDetails.buildingCode))
                serviceRequest.buildingCode = requestDetails.buildingCode;

            if (!string.IsNullOrEmpty(requestDetails.description))
                serviceRequest.description = requestDetails.description;

            if (requestDetails.currentStatus.HasValue)
                serviceRequest.currentStatus = requestDetails.currentStatus.Value;

            serviceRequest.lastModifiedBy = requestDetails.lastModifiedBy != null? requestDetails.lastModifiedBy : "";

            parameters.Add(DAConstants.DAServiceRequests.PARAM_REQUESTID, serviceRequest.id);
            parameters.Add(DAConstants.DAServiceRequests.PARAM_BUILDINGCODE, serviceRequest.buildingCode);
            parameters.Add(DAConstants.DAServiceRequests.PARAM_DESCRIPTION, serviceRequest.description);
            parameters.Add(DAConstants.DAServiceRequests.PARAM_CURRENTSTATUS, serviceRequest.currentStatus);
            parameters.Add(DAConstants.DAServiceRequests.PARAM_LASTMODIFIEDBY, serviceRequest.lastModifiedBy);

            return await _repository.UpdateRecord(parameters, DAConstants.DAServiceRequests.SN_EXECUTE_REQUESTS);
        }
        return 0;
    }

    /// <summary>
    /// Delete service request Infomation
    /// </summary>
    /// <param name="requestDetails">ServiceRequestDetailsRequest</param>
    /// <returns>int</returns>
    public async Task<int> DeleteServiceRequest(ServiceRequestDetailsRequest requestDetails)
    {
        DynamicParameters parameters = new();
        parameters.Add(DAConstants.DAServiceRequests.PARAM_REQUESTID, requestDetails.serviceRequestId);

        return await _repository.DeleteRecord(parameters, DAConstants.DAServiceRequests.SN_DELETE_REQUESTS);
    }
    #endregion
}

