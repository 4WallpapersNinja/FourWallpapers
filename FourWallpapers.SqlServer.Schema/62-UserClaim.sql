CREATE TABLE [dbo].[UserClaim]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[IdKey] [decimal](18, 0) IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
    CONSTRAINT [PK_UserClaim] PRIMARY KEY NONCLUSTERED ([Id]), 
	
    CONSTRAINT [PK_UserClaim_IdKey] UNIQUE CLUSTERED ([IdKey]), 
)
