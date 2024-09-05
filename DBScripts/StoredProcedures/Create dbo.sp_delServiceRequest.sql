USE [ServiceRequest]
GO

/****** Object: SqlProcedure [dbo].[sp_delServiceRequest] Script Date: 05-09-2024 16:39:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_delServiceRequest]
( 
	@RequestId UNIQUEIDENTIFIER = NULL
)
AS
	BEGIN
		DELETE dbo.ServiceRequest
		WHERE Id = @RequestId
	END
