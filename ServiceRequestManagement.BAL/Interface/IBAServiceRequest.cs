using ServiceRequestManagement.Models;

namespace ServiceRequestManagement.BAL.Interface;

public interface IBAServiceRequest
{
    Task<IEnumerable<ServiceRequestDetails>> GetServiceRequests();
    Task<ServiceRequestDetails> GetServiceRequestById(Guid serviceRequestId);
    Task<int> AddServiceRequest(AddServiceRequest requestDetails);
    Task<int> UpdateServiceRequest(UpdateServiceRequest requestDetails);
    Task<int> DeleteServiceRequest(ServiceRequestDetailsRequest requestDetails);

}
