CREATE TABLE [dbo].[Scrape]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [ImageId] NVARCHAR(250) NULL, 
    [Source] INT NULL, 
    [Hash] NVARCHAR(512) NULL,
    [ScrapeId] UNIQUEIDENTIFIER NULL,
	[Datestamp] DATETIME NOT NULL
)

GO

CREATE NONCLUSTERED INDEX [IX_Scrape_ImageId_Source] ON [dbo].[Scrape]
(
	[ImageId] ASC,
	[Source] ASC
)