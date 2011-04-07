CREATE TABLE [dbo].[CaseStudies] (
    [id]          INT             IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (100)   NOT NULL,
    [Description] VARCHAR (MAX)   NULL,
    [File]        VARBINARY (MAX) NOT NULL,
    [SeminarId]   INT             NOT NULL,
    [SessionId]   INT             NULL
);

