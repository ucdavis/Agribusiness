CREATE TABLE [dbo].[CaseStudies] (
    [id]          INT             IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (100)   NOT NULL,
    [Description] VARCHAR (MAX)   NULL,
    [File]        VARBINARY (MAX) NULL,
    [ContentType] VARCHAR (200)   NULL,
    [SeminarId]   INT             NOT NULL,
    [SessionId]   INT             NULL,
    [IsPublic]    BIT             NOT NULL,
    [DateCreated] DATETIME        NOT NULL,
    [LastUpdate]  DATETIME        NOT NULL
);



