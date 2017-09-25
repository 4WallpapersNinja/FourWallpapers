IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'Scrape'))
BEGIN
CREATE TABLE [dbo].[Scrape]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [ImageId] NVARCHAR(250) NULL, 
    [Source] INT NULL, 
    [Hash] NVARCHAR(512) NULL,
    [ScrapeId] UNIQUEIDENTIFIER NULL
)
END