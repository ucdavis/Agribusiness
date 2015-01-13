CREATE TABLE [dbo].[Templates] (
    [id]                 INT           IDENTITY (1, 1) NOT NULL,
    [BodyText]           VARCHAR (MAX) NOT NULL,
    [IsActive]           BIT           NOT NULL,
    [NotificationTypeId] CHAR (2)      NOT NULL, 
    [SeminarId] INT NOT NULL, 
    [DateCreated] DATETIME NOT NULL DEFAULT getdate(), 
    CONSTRAINT [FK_Templates_Seminars] FOREIGN KEY ([SeminarId]) REFERENCES [Seminars]([Id])
);

