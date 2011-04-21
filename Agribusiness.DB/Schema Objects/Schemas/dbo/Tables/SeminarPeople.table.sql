CREATE TABLE [dbo].[SeminarPeople] (
    [id]               INT              IDENTITY (1, 1) NOT NULL,
    [SeminarId]        INT              NOT NULL,
    [PersonId]         INT              NOT NULL,
    [Title]            VARCHAR (50)     NULL,
    [FirmCode]         UNIQUEIDENTIFIER NULL,
    [FirmId]           INT              NULL,
    [CouponCode]       VARCHAR (15)     NULL,
    [Registered]       BIT              NOT NULL,
    [RegistrationCode] AS               (right('0000'+CONVERT([varchar],[id],(0)),(6))),
    [Invite]           BIT              NOT NULL
);





