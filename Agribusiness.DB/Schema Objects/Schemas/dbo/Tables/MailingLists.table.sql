CREATE TABLE [dbo].[MailingLists] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (100) NOT NULL,
    [SeminarId]   INT           NOT NULL,
    [DateCreated] DATETIME      NOT NULL,
    [DateUpdated] DATETIME      NOT NULL
);

