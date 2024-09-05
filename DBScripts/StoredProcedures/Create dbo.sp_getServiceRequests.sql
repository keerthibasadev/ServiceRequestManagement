USE [ServiceRequest]
GO

/****** Object: SqlProcedure [dbo].[sp_getServiceRequests] Script Date: 05-09-2024 16:40:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_getServiceRequests]
(
	@_Mode INT = NULL,
	@RequestId UNIQUEIDENTIFIER = NULL
)
AS
BEGIN
	IF (@_Mode = 1) --Get Service request list
	BEGIN
		SELECT Id, BuildingCode, Description, CurrentStatus, CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate 
		FROM 
			ServiceRequest (NOLOCK)
	END
	ELSE IF (@_Mode = 2) --Get Service request details
	BEGIN
		SELECT Id, BuildingCode, Description, CurrentStatus, CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate 
		FROM 
			ServiceRequest (NOLOCK)
		WHERE Id = @RequestId
			
	END
END
