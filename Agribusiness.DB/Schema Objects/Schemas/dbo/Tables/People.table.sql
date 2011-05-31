﻿CREATE TABLE [dbo].[People] (
    [id]                    INT              IDENTITY (1, 1) NOT NULL,
    [LastName]              VARCHAR (50)     NOT NULL,
    [FirstName]             VARCHAR (50)     NOT NULL,
    [MI]                    VARCHAR (50)     NULL,
    [Salutation]            VARCHAR (5)      NULL,
    [BadgeName]             VARCHAR (50)     NULL,
    [Phone]                 VARCHAR (15)     NOT NULL,
    [PhoneExt]              VARCHAR (10)     NULL,
    [CellPhone]             VARCHAR (15)     NULL,
    [Fax]                   VARCHAR (15)     NULL,
    [Biography]             VARCHAR (MAX)    NULL,
    [Invite]                BIT              NOT NULL,
    [UserId]                UNIQUEIDENTIFIER NOT NULL,
    [OriginalPicture]       VARBINARY (MAX)  NULL,
    [MainProfilePicture]    VARBINARY (MAX)  NULL,
    [ThumbnailPicture]      VARBINARY (MAX)  NULL,
    [ContentType]           VARCHAR (20)     NULL,
    [CommunicationOptionId] CHAR (2)         NOT NULL,
    [AutomatedNotification] BIT              NOT NULL
);











