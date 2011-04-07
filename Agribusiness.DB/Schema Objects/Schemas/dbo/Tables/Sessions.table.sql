CREATE TABLE [dbo].[Sessions] (
    [id]            INT           IDENTITY (1, 1) NOT NULL,
    [Name]          VARCHAR (100) NOT NULL,
    [Location]      VARCHAR (50)  NULL,
    [Begin]         DATETIME      NULL,
    [End]           DATETIME      NULL,
    [SessionTypeId] CHAR (2)      NOT NULL,
    [SeminarId]     INT           NOT NULL
);

