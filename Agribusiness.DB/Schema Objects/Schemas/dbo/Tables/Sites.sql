CREATE TABLE [dbo].[Sites]
(
	[Id] VARCHAR(10) NOT NULL PRIMARY KEY, 
    [Name] VARCHAR(100) NOT NULL, 
	[Description] varchar(max) null,
    [IsActive] BIT NOT NULL DEFAULT 1, 
    [Logo] VARBINARY(MAX) NULL, 
	[LogoContentType] varchar(50) null,
    [SplashImage] VARBINARY(MAX) NULL,
	[SplashContentType] varchar(50) null
)
