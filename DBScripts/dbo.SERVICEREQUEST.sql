CREATE TABLE [dbo].[SERVICEREQUEST] (
    [Id]               UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [BuildingCode]     NVARCHAR (50)    NOT NULL,
    [Description]      NVARCHAR (MAX)   NOT NULL,
    [CurrentStatus]    INT              NOT NULL,
    [CreatedBy]        NVARCHAR (100)   NOT NULL,
    [CreatedDate]      DATETIME         NOT NULL,
    [LastModifiedBy]   NVARCHAR (100)   NOT NULL,
    [LastModifiedDate] DATETIME         NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

