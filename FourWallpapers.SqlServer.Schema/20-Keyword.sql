CREATE TABLE [dbo].[Keyword]
(
	[Id] [uniqueidentifier] NOT NULL,
	[IdKey] [decimal](18, 0) IDENTITY(1,1) NOT NULL,
    [Value] NVARCHAR(256) NULL,
    CONSTRAINT [PK_Keyword] PRIMARY KEY NONCLUSTERED ([Id]),
    CONSTRAINT [Key_Keyword] UNIQUE CLUSTERED ([IdKey]) 
)
