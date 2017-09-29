CREATE TABLE [dbo].[Report]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [UserId] UNIQUEIDENTIFIER NULL, 
    [Timestamp] DATETIME NOT NULL, 
    [IpAddress] NVARCHAR(128) NOT NULL, 
    [ImageIdKey] DECIMAL NOT NULL, 
    CONSTRAINT [FK_Report_Image] FOREIGN KEY ([ImageIdKey]) REFERENCES [Image]([IdKey])
	ON DELETE Cascade
	ON UPDATE Cascade
)
