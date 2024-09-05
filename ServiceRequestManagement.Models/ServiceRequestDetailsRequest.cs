using System.ComponentModel.DataAnnotations;

namespace ServiceRequestManagement.Models;

    public class ServiceRequestDetailsRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Service request id is required.")]
        public Guid? serviceRequestId { get; set; }
    }
