using System.ComponentModel.DataAnnotations;

namespace ServiceRequestManagement.Models;
public class AddServiceRequest
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Building code is required.")]
    public string? buildingCode { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Description is required.")]
    public string? description { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Current status is required.")]
    public CurrentStatus? currentStatus { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Created by is required.")]
    public string? createdBy { get; set; }
}