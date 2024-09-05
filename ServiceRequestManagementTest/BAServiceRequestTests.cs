using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using ServiceRequestManagement.BAL;
using ServiceRequestManagement.DAL.Interface;
using ServiceRequestManagement.Models;
using Dapper;
using ServiceRequestManagement.DAL;

namespace ServiceRequestManagement.Tests
{
    public class BAServiceRequestTests
    {
        private readonly Mock<IDapperContext> _mockContext;
        private readonly Mock<IRepository> _mockRepository;

        public BAServiceRequestTests()
        {
            _mockContext = new Mock<IDapperContext>();
            _mockRepository = new Mock<IRepository>();

            _mockContext.Setup(c => c.CreateServiceConnection()).Returns(new Mock<System.Data.IDbConnection>().Object);
        }

        [Fact]
        public async Task GetServiceRequests_ShouldReturnListOfServiceRequestDetails()
        {
            // Arrange
            var expectedRequests = new List<ServiceRequestDetails>
            {
                new ServiceRequestDetails { id = Guid.NewGuid(), buildingCode = "KVH", description = "Request 1" },
                new ServiceRequestDetails { id = Guid.NewGuid(), buildingCode = "RP", description = "Request 2" }
            };

            _mockRepository.Setup(r => r.GetAllRecords<ServiceRequestDetails>(
                It.IsAny<DynamicParameters>(),
                It.Is<string>(s => s == DAConstants.DAServiceRequests.SN_RETRIEVE_REQUESTS)))
                .ReturnsAsync(expectedRequests);

            var baServiceRequest = new BAServiceRequest(_mockContext.Object);
            SetPrivateField(baServiceRequest, "_repository", _mockRepository.Object);

            // Act
            var result = await baServiceRequest.GetServiceRequests();

            // Assert
            Assert.Equal(expectedRequests, result);
        }

        [Fact]
        public async Task GetServiceRequestById_ShouldReturnServiceRequestDetails()
        {
            // Arrange
            var expectedRequest = new ServiceRequestDetails
            {
                id = Guid.NewGuid(),
                buildingCode = "BC1",
                description = "Test Request"
            };

            _mockRepository.Setup(r => r.GetRecord<ServiceRequestDetails>(
                It.IsAny<DynamicParameters>(),
                It.Is<string>(s => s == DAConstants.DAServiceRequests.SN_RETRIEVE_REQUESTS)))
                .ReturnsAsync(expectedRequest);

            var baServiceRequest = new BAServiceRequest(_mockContext.Object);
            SetPrivateField(baServiceRequest, "_repository", _mockRepository.Object);

            // Act
            var result = await baServiceRequest.GetServiceRequestById(expectedRequest.id);

            // Assert
            Assert.Equal(expectedRequest, result);
        }

        [Fact]
        public async Task AddServiceRequest_ShouldReturnInsertedId()
        {
            // Arrange
            var newRequest = new AddServiceRequest
            {
                buildingCode = "BC3",
                description = "New Request",
                currentStatus = CurrentStatus.CREATED,
                createdBy = "TestUser"
            };

            int expectedInsertedId = 1;

            _mockRepository.Setup(r => r.InsertRecord(
                It.IsAny<DynamicParameters>(),
                It.Is<string>(s => s == DAConstants.DAServiceRequests.SN_EXECUTE_REQUESTS)))
                .ReturnsAsync(expectedInsertedId);

            var baServiceRequest = new BAServiceRequest(_mockContext.Object);
            SetPrivateField(baServiceRequest, "_repository", _mockRepository.Object);

            // Act
            var result = await baServiceRequest.AddServiceRequest(newRequest);

            // Assert
            Assert.Equal(expectedInsertedId, result);
        }

        [Fact]
        public async Task UpdateServiceRequest_ExistingRequest_ShouldReturnUpdatedRowCount()
        {
            // Arrange
            var updateRequest = new UpdateServiceRequest
            {
                serviceRequestId = Guid.NewGuid(),
                buildingCode = "KV",
                description = "Updated Request",
                currentStatus = CurrentStatus.INPROGRESS,
                lastModifiedBy = "TestUser"
            };

            var existingRequest = new ServiceRequestDetails
            {
                id = updateRequest.serviceRequestId,
                buildingCode = "RPH",
                description = "Original Request",
                currentStatus = CurrentStatus.NOTAPPLICABLE,
                lastModifiedBy = "OriginalUser"
            };

            int expectedUpdatedRowCount = 1;

            _mockRepository.Setup(r => r.GetRecord<ServiceRequestDetails>(
                It.IsAny<DynamicParameters>(),
                It.Is<string>(s => s == DAConstants.DAServiceRequests.SN_RETRIEVE_REQUESTS)))
                .ReturnsAsync(existingRequest);

            _mockRepository.Setup(r => r.UpdateRecord(
                It.IsAny<DynamicParameters>(),
                It.Is<string>(s => s == DAConstants.DAServiceRequests.SN_EXECUTE_REQUESTS)))
                .ReturnsAsync(expectedUpdatedRowCount);

            var baServiceRequest = new BAServiceRequest(_mockContext.Object);
            SetPrivateField(baServiceRequest, "_repository", _mockRepository.Object);

            // Act
            var result = await baServiceRequest.UpdateServiceRequest(updateRequest);

            // Assert
            Assert.Equal(expectedUpdatedRowCount, result);
        }

        [Fact]
        public async Task UpdateServiceRequest_NonExistingRequest_ShouldReturnZero()
        {
            // Arrange
            var updateRequest = new UpdateServiceRequest
            {
                serviceRequestId = Guid.NewGuid(),
                buildingCode = "KV",
                description = "Updated Request",
                currentStatus = CurrentStatus.CREATED,
                lastModifiedBy = "TestUser"
            };

            _mockRepository.Setup(r => r.GetRecord<ServiceRequestDetails>(
                It.IsAny<DynamicParameters>(),
                It.Is<string>(s => s == DAConstants.DAServiceRequests.SN_RETRIEVE_REQUESTS)))
                .ReturnsAsync((ServiceRequestDetails)null);

            var baServiceRequest = new BAServiceRequest(_mockContext.Object);
            SetPrivateField(baServiceRequest, "_repository", _mockRepository.Object);

            // Act
            var result = await baServiceRequest.UpdateServiceRequest(updateRequest);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public async Task DeleteServiceRequest_ShouldReturnDeletedRowCount()
        {
            // Arrange
            var deleteRequest = new ServiceRequestDetailsRequest
            {
                serviceRequestId = Guid.NewGuid()
            };

            int expectedDeletedRowCount = 1;

            _mockRepository.Setup(r => r.DeleteRecord(
                It.IsAny<DynamicParameters>(),
                It.Is<string>(s => s == DAConstants.DAServiceRequests.SN_DELETE_REQUESTS)))
                .ReturnsAsync(expectedDeletedRowCount);

            var baServiceRequest = new BAServiceRequest(_mockContext.Object);
            SetPrivateField(baServiceRequest, "_repository", _mockRepository.Object);

            // Act
            var result = await baServiceRequest.DeleteServiceRequest(deleteRequest);

            // Assert
            Assert.Equal(expectedDeletedRowCount, result);
        }

        // Helper method to set private fields for testing
        private static void SetPrivateField(object instance, string fieldName, object value)
        {
            var field = instance.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field?.SetValue(instance, value);
        }
    }
}