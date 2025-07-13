CREATE SCHEMA dto;
GO

CREATE TABLE [dto].[OtpRequest] (
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [Email] NVARCHAR(200) NOT NULL,
    [Phone] NVARCHAR(20) NULL,
    [OtpCode] NVARCHAR(64) NOT NULL, -- Stores SHA256 hash of OTP + salt (hex encoded)
    [ExpiresAt] DATETIME2 NOT NULL,
    [IsUsed] BIT NOT NULL DEFAULT 0,
    [Created] DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),
    [LastModified] DATETIMEOFFSET NULL
);
GO

CREATE NONCLUSTERED INDEX IX_OtpRequest_Email ON [dto].[OtpRequest] ([Email]);
GO

ALTER TABLE [dto].[OtpRequest]
ADD CONSTRAINT UQ_OtpRequest_Email_OtpCode UNIQUE ([Email], [OtpCode]);
GO
