CREATE TABLE [dbo].[Sites]
(
	[Id] VARCHAR(10) NOT NULL PRIMARY KEY, 
    [Name] VARCHAR(100) NOT NULL, 
	[Description] varchar(max) null,
	[Welcome] varchar(max) null,
    [IsActive] BIT NOT NULL DEFAULT 1, 
    [Logo] VARBINARY(MAX) NULL, 
	[LogoContentType] varchar(50) null,
    [SplashImage] VARBINARY(MAX) NULL,
	[SplashContentType] varchar(50) null, 
    [EventType] VARCHAR(50) NULL, 
    [CollectExtended] BIT NOT NULL DEFAULT 0, 
    [Subdomain] VARCHAR(50) NOT NULL, 
    [Background] VARCHAR(MAX) NULL, 
	[BackgroundPersonId] int null,
    [Venue] VARCHAR(MAX) NULL, 
    [VenueEmbeddedMap] VARCHAR(MAX) NULL
)
