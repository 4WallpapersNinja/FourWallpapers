INSERT INTO [dbo].[Scrape]
           ([Id]
           ,[ImageId]
           ,[Source]
           ,[Hash]
           ,[ScrapeId]
		   ,[Datestamp])
     VALUES
           (@Id
           ,@ImageId
           ,@Source
           ,@Hash
           ,@ScrapeId
		   ,@Datestamp)