using System.ComponentModel.DataAnnotations;

namespace ServiceRequestManagement.Models;
public class UpdateServiceRequest
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Service request id is required.")]
    public Guid serviceRequestId { get; set; }
    public string? buildingCode { get; set; }
    public string? description { get; set; }
    public CurrentStatus? currentStatus { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Last modified by is required.")]
    public string? lastModifiedBy { get; set; }
}