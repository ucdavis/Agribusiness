CREATE TABLE [dbo].[Firms] (
    [id]          INT              IDENTITY (1, 1) NOT NULL,
    [FirmCode]    UNIQUEIDENTIFIER NOT NULL,
    [Name]        VARCHAR (200)    NOT NULL,
    [Description] VARCHAR (MAX)    NOT NULL,
    [Review]      BIT              NOT NULL,
    [WebAddress]  VARCHAR (200)    NULL,
    [archive_id]  UNIQUEIDENTIFIER NULL
);





