CREATE TABLE [dbo].[Image]
(
	[Id] [uniqueidentifier] NOT NULL,
	[IdKey] [decimal](18, 0) IDENTITY(1,1) NOT NULL,
	[ImageId] [nvarchar](512) NULL,
	[FileExtension] [nvarchar](128) NULL,
	[Class] [int] NULL,
	[IndexSource] [int] NULL,
	[Who] [nvarchar](1024) NULL,
	[ResolutionX] [int] NULL,
	[ResolutionY] [int] NULL,
	[TagsString] [nvarchar](max) NULL,
	[DateDownloaded] [datetime] NULL,
	[Reported] [int] NULL,
	[Ratio] [nvarchar](12) NULL,
	[Downloads] [bigint] NULL,
	[Hash] [nvarchar](512) NULL,
	[ServerId] [int] NULL,
	[IsThumbnailAvailable] [bit] NULL,
	[IsLockedImage] [bit] NULL,
	[Size] [decimal](18, 0) NULL,
    CONSTRAINT [PK_Image] PRIMARY KEY NONCLUSTERED ([Id]), 
    CONSTRAINT [Key_Image] UNIQUE CLUSTERED ([IdKey])
)
GO
CREATE UNIQUE INDEX [IX_ImageId] ON [dbo].[Image] ([ImageId])

