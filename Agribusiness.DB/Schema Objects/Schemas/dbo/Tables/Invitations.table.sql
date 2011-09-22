CREATE TABLE [dbo].[Invitations] (
    [id]        INT           IDENTITY (1, 1) NOT NULL,
    [PersonId]  INT           NOT NULL,
    [Title]     VARCHAR (200) NULL,
    [FirmName]  VARCHAR (200) NULL,
    [SeminarId] INT           NOT NULL
);

