using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ServiceRequestManagement.BAL.Interface;
using ServiceRequestManagement.Common;
using ServiceRequestManagement.Models;
using ServiceRequestManagement.Utils.Middleware;

namespace ServiceRequestManagement.Controllers;

    public class ServiceRequestController : Controller
    {
    private readonly ILogger<ServiceRequestController> _logger;
    private readonly IBAServiceRequest _baServiceRequest;
    public ServiceRequestController(ILogger<ServiceRequestController> logger, IBAServiceRequest baServiceRequest)
    {
        _logger = logger;
        _baServiceRequest = baServiceRequest;
    }

    // [Authorize]
    [HttpGet("GetServiceRequestList")]
    public async Task<IActionResult> GetServiceRequestList()
    {
        try
        {
            IEnumerable<ServiceRequestDetails> requestDetails = await _baServiceRequest.GetServiceRequests();
            return Ok(requestDetails);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetServiceRequestList Error Message: {Message}", ex.Message);
            throw new CustomException(ex.Message, ex);
        }
    }

    [HttpGet("GetServiceRequestDetails/{serviceRequestId?}")]
    public async Task<IActionResult> GetServiceRequestDetails([FromRoute] ServiceRequestDetailsRequest requestDetails)
    {
        try
        {          
            if (requestDetails.serviceRequestId == null)
            {
                return BadRequest(ErrorResponseUtility.BadRequestResponse("Service request id is required."));
            }

            ServiceRequestDetails serviceRequestDetail = await _baServiceRequest.GetServiceRequestById(requestDetails.serviceRequestId.Value);
            return Ok(serviceRequestDetail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetServiceRequestDetails Error Message: {Message}", ex.Message);
            throw new CustomException(ex.Message, ex);
        }
    }

    [HttpPost("AddServiceRequest")]
    public async Task<IActionResult> AddServiceRequest([FromBody] AddServiceRequest requestDetails)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponseUtility.BadRequestResponse());

           int result = await _baServiceRequest.AddServiceRequest(requestDetails);
            if (result <= 0)
                return UnprocessableEntity(ErrorResponseUtility.UnprocessableEntityResponse(AppMessages.ERR));

            BaseSuccessResponse successResponse = new(AppMessages.SAVE_REQUEST_SUCCESS);
            
            return Ok(successResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AddServiceRequest Error Message: {Message}", ex.Message);
            throw new CustomException(ex.Message, ex);
        }
    }

    [HttpPut("UpdateServiceRequest/{serviceRequestId}")]
    public async Task<IActionResult> UpdateServiceRequest(Guid serviceRequestId, [FromBody] UpdateServiceRequest requestDetails)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponseUtility.BadRequestResponse());

            if (serviceRequestId != requestDetails.serviceRequestId)
                return BadRequest(ErrorResponseUtility.BadRequestResponse("Invalid service request id."));

            int result = await _baServiceRequest.UpdateServiceRequest(requestDetails);
            if (result <= 0)
                return UnprocessableEntity(ErrorResponseUtility.UnprocessableEntityResponse(AppMessages.ERR));

            BaseSuccessResponse successResponse = new(AppMessages.UPDATE_REQUEST_SUCCESS);
            return Ok(successResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UpdateServiceRequest Error Message: {Message}", ex.Message);
            throw new CustomException(ex.Message, ex);
        }
    }

    [HttpDelete("DeleteServiceRequest/{serviceRequestId?}")]
    public async Task<IActionResult> DeleteServiceRequest([FromRoute] ServiceRequestDetailsRequest requestDetails)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponseUtility.BadRequestResponse());

            int result = await _baServiceRequest.DeleteServiceRequest(requestDetails);
            if (result <= 0)
                return UnprocessableEntity(ErrorResponseUtility.UnprocessableEntityResponse(AppMessages.ERR));

            BaseSuccessResponse successResponse = new(AppMessages.DELETE_REQUEST_SUCCESS);

            return Ok(successResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DeleteServiceRequest Error Message: {Message}", ex.Message);
            throw new CustomException(ex.Message, ex);
        }
    }
}

