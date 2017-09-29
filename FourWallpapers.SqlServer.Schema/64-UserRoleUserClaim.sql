CREATE TABLE [dbo].[UserRoleUserClaim]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	[UserRoleIdKey] [decimal](18, 0) NOT NULL,
	[UserClaimIdKey] [decimal](18, 0) NOT NULL,
	  CONSTRAINT [FK_UserClaim] FOREIGN KEY ([UserRoleIdKey]) REFERENCES [UserRole]([IdKey])
		ON DELETE CASCADE    
    ON UPDATE CASCADE    ,
	CONSTRAINT [FK_UserRoleId] FOREIGN KEY ([UserClaimIdKey]) REFERENCES [UserClaim]([IdKey]) 
		ON DELETE CASCADE    
    ON UPDATE CASCADE    
)
