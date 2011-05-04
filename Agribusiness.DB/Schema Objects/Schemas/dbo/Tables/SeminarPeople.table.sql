CREATE TABLE [dbo].[SeminarPeople] (
    [id]                        INT              IDENTITY (1, 1) NOT NULL,
    [SeminarId]                 INT              NOT NULL,
    [PersonId]                  INT              NOT NULL,
    [Title]                     VARCHAR (50)     NOT NULL,
    [FirmId]                    INT              NOT NULL,
    [CouponCode]                VARCHAR (15)     NULL,
    [CouponAmount]              MONEY            NULL,
    [Paid]                      BIT              NOT NULL,
    [ReferenceId]               AS               (right('0000'+CONVERT([varchar],[id],(0)),(6))),
    [Invite]                    BIT              NOT NULL,
    [ContactInformationRelease] BIT              NOT NULL,
    [TransactionId]             VARCHAR (20)     NULL,
    [Comments]                  VARCHAR (MAX)    NULL,
    [archive_id]                UNIQUEIDENTIFIER NULL
);

















