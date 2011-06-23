CREATE TABLE [dbo].[EmailQueue] (
    [id]           INT           IDENTITY (1, 1) NOT NULL,
    [PersonId]     INT           NOT NULL,
    [Created]      DATETIME      NOT NULL,
    [Pending]      BIT           NOT NULL,
    [SentDateTime] DATETIME      NULL,
    [Subject]      VARCHAR (100) NULL,
    [Body]         VARCHAR (MAX) NULL,
    [ErrorCode]    INT           NULL,
    [FromAddress]  VARCHAR (50)  NULL
);



