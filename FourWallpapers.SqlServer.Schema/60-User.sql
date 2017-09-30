CREATE TABLE [dbo].[User]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[IdKey] [decimal](18, 0) IDENTITY(1,1) NOT NULL,	
    [Email] NVARCHAR(255) NOT NULL, 
    [UserName] NVARCHAR(255) NOT NULL, 
    [PasswordHash] NVARCHAR(255) NOT NULL, 
    [PasswordSalt] NVARCHAR(255) NOT NULL, 
    [TwoFactorAuth] NVARCHAR(255) NOT NULL, 
    CONSTRAINT [PK_User] PRIMARY KEY NONCLUSTERED ([Id]), 
    CONSTRAINT [PK_User_IdKey] UNIQUE CLUSTERED ([IdKey])
)

GO

CREATE INDEX [IX_User_Username] ON [dbo].[User] ([UserName])

GO

CREATE INDEX [IX_User_Email] ON [dbo].[User] ([Email])
GO
