CREATE TABLE [dbo].[Sponsors]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Name] VARCHAR(100) NOT NULL, 
    [Logo] VARBINARY(MAX) NULL, 
    [LogoContentType] VARCHAR(50) NULL, 
    [Url] VARCHAR(MAX) NULL, 
    [Level] VARCHAR(10) NULL, 
    [Description] VARCHAR(MAX) NULL, 
    [IsActive] BIT NOT NULL DEFAULT 1, 
    [Order] INT NOT NULL DEFAULT 0, 
    [SiteId] VARCHAR(10) NOT NULL, 
    CONSTRAINT [FK_Sponsors_Sites] FOREIGN KEY ([SiteId]) REFERENCES [Sites]([Id])
)
