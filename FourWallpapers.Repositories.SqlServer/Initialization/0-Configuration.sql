IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'Configuration'))
BEGIN
CREATE TABLE [dbo].[Configuration]
(
	[Id] NVARCHAR(50) NOT NULL PRIMARY KEY, 
    [Value] NVARCHAR(MAX) NULL
)
END