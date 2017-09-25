IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'UpdateRequest'))
BEGIN
CREATE TABLE [dbo].[UpdateRequest]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [UserId] UNIQUEIDENTIFIER NULL, 
    [IpAddress] NVARCHAR(128) NOT NULL, 
    [Key] NVARCHAR(16) NOT NULL, 
    [Value] NVARCHAR(512) NOT NULL
)
END