CREATE TABLE [dbo].[Seminars] (
    [id]                   INT           IDENTITY (1, 1) NOT NULL,
    [Year]                 INT           NOT NULL,
    [Location]             VARCHAR (100) NOT NULL,
    [Begin]                DATETIME      NOT NULL,
    [End]                  DATETIME      NOT NULL,
    [RegistrationPassword] VARCHAR (20)  NULL,
    [RegistrationId]       INT           NULL,
    [RegistrationBegin]    DATE          NULL,
    [RegistrationDeadline] DATE          NULL,
    [ReleaseToAttendees]   BIT           NOT NULL
);



