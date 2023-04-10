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

CREATE TABLE [APIs]
(
	[Id] BIGINT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	[PublicKey] NVARCHAR(200) NULL,
	[PrivateKey] NVARCHAR(200) NULL,

	[Deleted] BIT NOT NULL DEFAULT 0,
	[UpdateDate] DATETIME2 NULL,
	[CreateDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);
GO

CREATE TABLE [UserAPIs]
(
	[Id] BIGINT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	[UserId] BIGINT NULL FOREIGN KEY REFERENCES [Users]([Id]),
	[ApiId] BIGINT NULL FOREIGN KEY REFERENCES [APIs]([Id]),
	[Name] NVARCHAR(200) NOT NULL,
	[Permissions] INT NOT NULL DEFAULT 0,

	[Deleted] BIT NOT NULL DEFAULT 0,
	[UpdateDate] DATETIME2 NULL,
	[CreateDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);
GO

CREATE TABLE [Permissions]
(
	[Id] BIGINT NOT NULL PRIMARY KEY,
	[Name] NVARCHAR(200) NOT NULL,
);
GO