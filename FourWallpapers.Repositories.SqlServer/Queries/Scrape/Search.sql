SELECT COUNT(*)
FROM [dbo].[Scrape]
WHERE 
    [ImageId] = @ImageId
    AND [Source] = @Source