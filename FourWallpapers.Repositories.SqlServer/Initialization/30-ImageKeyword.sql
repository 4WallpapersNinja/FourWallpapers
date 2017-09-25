IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'ImageKeyword'))
BEGIN
CREATE TABLE [dbo].[ImageKeyword]
(
	[ImageIdKey] decimal(18,0) NOT NULL, 
    [KeywordIdKey] decimal(18,0) NOT NULL, 
    CONSTRAINT [PK_Keywords] PRIMARY KEY CLUSTERED ([ImageIdKey], [KeywordIdKey]), 
    CONSTRAINT [FK_Image] FOREIGN KEY ([ImageIdKey]) REFERENCES [Image]([IdKey])
		ON DELETE CASCADE    
    ON UPDATE CASCADE    ,
	CONSTRAINT [FK_Keyword] FOREIGN KEY ([KeywordIdKey]) REFERENCES [Keyword]([IdKey]) 
		ON DELETE CASCADE    
    ON UPDATE CASCADE    
)
END