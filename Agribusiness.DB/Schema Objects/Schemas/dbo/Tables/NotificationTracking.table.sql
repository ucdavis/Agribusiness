CREATE TABLE [dbo].[NotificationTracking] (
    [id]                   INT           IDENTITY (1, 1) NOT NULL,
    [PersonId]             INT           NULL,
    [ApplicationId]        INT           NULL,
    [NotificationMethodId] CHAR (1)      NOT NULL,
    [NotificationTypeId]   CHAR (2)      NOT NULL,
    [DateTime]             DATETIME      NOT NULL,
    [NotifiedBy]           VARCHAR (50)  NOT NULL,
    [Comments]             VARCHAR (MAX) NULL,
    [SeminarId]            INT           NOT NULL,
    [EmailQueueId]         INT           NULL
);







