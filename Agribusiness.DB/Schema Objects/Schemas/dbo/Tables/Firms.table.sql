CREATE TABLE [dbo].[Firms] (
    [id]          INT              IDENTITY (1, 1) NOT NULL,
    [FirmCode]    UNIQUEIDENTIFIER NOT NULL,
    [Name]        VARCHAR (200)    NOT NULL,
    [Description] VARCHAR (MAX)    NOT NULL,
    [Review]      BIT              NOT NULL
);

