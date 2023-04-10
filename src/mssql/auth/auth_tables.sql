--CREATE
--DATABASE identitydb
--GO
--USE [identitydb]
--GO

CREATE TABLE [Users]
(
	[Id] BIGINT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	[UserName] NVARCHAR(200) NOT NULL,
	[Salt] NVARCHAR(255) NULL,
	[Password] NVARCHAR(255) NOT NULL,

	[Deleted] BIT NOT NULL DEFAULT 0,
	[UpdateDate] DATETIME2 NULL,
	[CreateDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);
GO
