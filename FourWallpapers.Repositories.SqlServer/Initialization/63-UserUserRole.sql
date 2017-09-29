CREATE TABLE [dbo].[UserUserRole]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	[UserIdKey] [decimal](18, 0) NOT NULL,
	[UserRoleIdKey] [decimal](18, 0) NOT NULL,
	  CONSTRAINT [FK_User] FOREIGN KEY ([UserIdKey]) REFERENCES [User]([IdKey])
		ON DELETE CASCADE    
    ON UPDATE CASCADE    ,
	CONSTRAINT [FK_UserRole] FOREIGN KEY ([UserRoleIdKey]) REFERENCES [UserRole]([IdKey]) 
		ON DELETE CASCADE    
    ON UPDATE CASCADE    
)
