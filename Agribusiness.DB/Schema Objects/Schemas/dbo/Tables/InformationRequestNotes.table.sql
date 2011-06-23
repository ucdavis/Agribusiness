CREATE TABLE [dbo].[InformationRequestNotes] (
    [Id]                   INT           IDENTITY (1, 1) NOT NULL,
    [InformationRequestId] INT           NOT NULL,
    [Notes]                VARCHAR (MAX) NOT NULL,
    [DateTimeNote]         DATETIME      NOT NULL,
    [Noteby]               VARCHAR (15)  NOT NULL
);

