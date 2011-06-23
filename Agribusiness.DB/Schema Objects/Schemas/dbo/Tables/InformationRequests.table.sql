CREATE TABLE [dbo].[InformationRequests] (
    [Id]                INT           IDENTITY (1, 1) NOT NULL,
    [Name]              VARCHAR (100) NOT NULL,
    [Title]             VARCHAR (75)  NOT NULL,
    [Company]           VARCHAR (75)  NOT NULL,
    [Email]             VARCHAR (50)  NOT NULL,
    [Commodity]         VARCHAR (200) NOT NULL,
    [Location]          VARCHAR (50)  NOT NULL,
    [SeminarId]         INT           NOT NULL,
    [SubmittedDateTime] DATETIME      NOT NULL,
    [Responded]         BIT           NOT NULL
);

