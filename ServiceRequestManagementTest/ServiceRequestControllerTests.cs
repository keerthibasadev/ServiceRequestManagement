using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ServiceRequestManagement.BAL.Interface;
using ServiceRequestManagement.Controllers;
using ServiceRequestManagement.Models;
using Xunit;

namespace ServiceRequestManagement.Tests;
public class ServiceRequestControllerTests
{
    private readonly Mock<ILogger<ServiceRequestController>> _mockLogger;
    private readonly Mock<IBAServiceRequest> _mockBAServiceRequest;
    private readonly ServiceRequestController _controller;

    public ServiceRequestControllerTests()
    {
        _mockLogger = new Mock<ILogger<ServiceRequestController>>();
        _mockBAServiceRequest = new Mock<IBAServiceRequest>();
        _controller = new ServiceRequestController(_mockLogger.Object, _mockBAServiceRequest.Object);
    }

    [Fact]
    public async Task GetServiceRequestList_ReturnsOkResult_WithServiceRequestDetails()
    {
        // Arrange
        var expectedServiceRequests = new List<ServiceRequestDetails>
            {
                new ServiceRequestDetails { id = Guid.NewGuid(), buildingCode = "BC001", description="test1",currentStatus=CurrentStatus.INPROGRESS, createdBy="Keerthi" },
                new ServiceRequestDetails { id = Guid.NewGuid(), buildingCode = "BC002", description="test2", currentStatus=CurrentStatus.CREATED, createdBy="Kee" }
            };
        _mockBAServiceRequest.Setup(x => x.GetServiceRequests()).ReturnsAsync(expectedServiceRequests);

        // Act
        var result = await _controller.GetServiceRequestList();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedServiceRequests = Assert.IsAssignableFrom<IEnumerable<ServiceRequestDetails>>(okResult.Value);
        Assert.Equal(expectedServiceRequests.Count, returnedServiceRequests.Count());
    }

    [Fact]
    public async Task GetServiceRequestDetails_WithValidId_ReturnsOkResult()
    {
        // Arrange
        var serviceRequestId = Guid.NewGuid();
        var expectedServiceRequest = new ServiceRequestDetails { id = serviceRequestId, buildingCode = "BC001", description = "test1", currentStatus = CurrentStatus.INPROGRESS, createdBy = "Keerthi" };
        _mockBAServiceRequest.Setup(x => x.GetServiceRequestById(serviceRequestId)).ReturnsAsync(expectedServiceRequest);

        // Act
        var result = await _controller.GetServiceRequestDetails(new ServiceRequestDetailsRequest { serviceRequestId = serviceRequestId });

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedServiceRequest = Assert.IsType<ServiceRequestDetails>(okResult.Value);
        Assert.Equal(serviceRequestId, returnedServiceRequest.id);
    }

    [Fact]
    public async Task AddServiceRequest_WithValidRequest_ReturnsOkResult()
    {
        // Arrange
        var addServiceRequest = new AddServiceRequest
        {
            buildingCode = "BC001",
            description = "Test request",
            currentStatus = CurrentStatus.CREATED,
            createdBy = "TestUser"
        };
        _mockBAServiceRequest.Setup(x => x.AddServiceRequest(addServiceRequest)).ReturnsAsync(1);

        // Act
        var result = await _controller.AddServiceRequest(addServiceRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var successResponse = Assert.IsType<BaseSuccessResponse>(okResult.Value);
        Assert.Equal("Service request saved successfully.", successResponse.message);
    }
}