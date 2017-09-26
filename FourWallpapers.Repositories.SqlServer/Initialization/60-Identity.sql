IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'IdentityLogin'))
BEGIN
CREATE TABLE [IdentityLogin](
	[LoginProvider] [varchar](128) NOT NULL,
	[ProviderKey] [varchar](128) NOT NULL,
	[UserId] [int] NOT NULL,
	[Name] [varchar](256) NOT NULL,
 CONSTRAINT [PK_IdentityLogin] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END

IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'IdentityRole'))
BEGIN
CREATE TABLE [IdentityRole](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_IdentityRole] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END

IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'IdentityUser'))
BEGIN
CREATE TABLE [IdentityUser](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](256) NOT NULL,
	[Email] [varchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [varchar](max) NULL,
	[SecurityStamp] [varchar](38) NULL,
	[PhoneNumber] [varchar](50) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
 CONSTRAINT [PK_IdentityUser] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'IdentityUserClaim'))
BEGIN
CREATE TABLE [IdentityUserClaim](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[ClaimType] [varchar](256) NOT NULL,
	[ClaimValue] [varchar](256) NULL,
 CONSTRAINT [PK_IdentityUserClaim] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END

IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'IdentityUserRole'))
BEGIN
CREATE TABLE [IdentityUserRole](
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
 CONSTRAINT [PK_IdentityUserRole] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END


ALTER TABLE [IdentityLogin]  WITH CHECK ADD  CONSTRAINT [FK_dbo.IdentityLogin_dbo.IdentityUser_UserId] FOREIGN KEY([UserId])
REFERENCES [IdentityUser] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [IdentityLogin] CHECK CONSTRAINT [FK_dbo.IdentityLogin_dbo.IdentityUser_UserId]
GO
ALTER TABLE [IdentityUserClaim]  WITH CHECK ADD  CONSTRAINT [FK_IdentityUserClaim_IdentityUser] FOREIGN KEY([UserId])
REFERENCES [IdentityUser] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [IdentityUserClaim] CHECK CONSTRAINT [FK_IdentityUserClaim_IdentityUser]
GO
ALTER TABLE [IdentityUserRole]  WITH CHECK ADD  CONSTRAINT [FK_IdentityUserRole_IdentityRole] FOREIGN KEY([RoleId])
REFERENCES [IdentityRole] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [IdentityUserRole] CHECK CONSTRAINT [FK_IdentityUserRole_IdentityRole]
GO
ALTER TABLE [IdentityUserRole]  WITH CHECK ADD  CONSTRAINT [FK_IdentityUserRole_IdentityUser] FOREIGN KEY([UserId])
REFERENCES [IdentityUser] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [IdentityUserRole] CHECK CONSTRAINT [FK_IdentityUserRole_IdentityUser]
GO
