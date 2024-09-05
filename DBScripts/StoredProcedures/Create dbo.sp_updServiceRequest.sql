USE [ServiceRequest]
GO

/****** Object: SqlProcedure [dbo].[sp_updServiceRequest] Script Date: 05-09-2024 16:40:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[sp_updServiceRequest]
(
    @_Mode INT = NULL,
	@RequestId UNIQUEIDENTIFIER = NULL,
	@BuildingCode NVARCHAR(50) = NULL,
	@Description NVARCHAR(MAX) = NULL,
	@CurrentStatus INT = NULL,
	@CreatedBy NVARCHAR(100) = NULL,
	@ModifiedBy NVARCHAR(100) = NULL
	)
AS
BEGIN
	IF (@_Mode = 1)--Add new service request
	BEGIN
		INSERT INTO dbo.ServiceRequest  (BuildingCode, Description, CurrentStatus, 
			CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate)
		VALUES (@BuildingCode, @Description, @CurrentStatus, 
			@CreatedBy,  GETDATE(), @CreatedBy,  GETDATE());
	END
	ELSE IF (@_Mode = 2)--Update service request
	BEGIN
		UPDATE dbo.ServiceRequest 
		SET BuildingCode = @BuildingCode,
			Description = @Description,
			CurrentStatus = @CurrentStatus,
			LastModifiedBy = @ModifiedBy,
			LastModifiedDate = GETDATE()
		WHERE Id = @RequestId
	END
END
