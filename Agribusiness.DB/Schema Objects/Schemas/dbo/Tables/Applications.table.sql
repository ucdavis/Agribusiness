﻿CREATE TABLE [dbo].[Applications] (
    [id]                        INT              IDENTITY (1, 1) NOT NULL,
    [FirstName]                 VARCHAR (50)     NOT NULL,
    [MI]                        VARCHAR (50)     NULL,
    [LastName]                  VARCHAR (50)     NOT NULL,
    [BadgeName]                 VARCHAR (50)     NULL,
    [Photo]                     VARBINARY (MAX)  NULL,
    [ContentType]               VARCHAR (50)     NULL,
    [AssistantName]             VARCHAR (100)    NULL,
    [AssistantPhone]            VARCHAR (15)     NULL,
    [AssistantEmail]            VARCHAR (50)     NULL,
    [Expectations]              VARCHAR (MAX)    NULL,
    [FirmId]                    INT              NULL,
    [FirmName]                  VARCHAR (200)    NULL,
    [FirmDescription]           VARCHAR (MAX)    NULL,
    [FirmAddressLine1]          VARCHAR (100)    NULL,
    [FirmAddressLine2]          VARCHAR (100)    NULL,
    [FirmCity]                  VARCHAR (50)     NULL,
    [FirmState]                 CHAR (2)         NULL,
    [FirmZip]                   VARCHAR (10)     NULL,
    [FirmPhone]                 VARCHAR (15)     NULL,
    [Website]                   VARCHAR (200)    NULL,
    [Responsibilities]          VARCHAR (MAX)    NOT NULL,
    [JobTitle]                  VARCHAR (50)     NOT NULL,
    [UserId]                    UNIQUEIDENTIFIER NOT NULL,
    [SeminarId]                 INT              NOT NULL,
    [IsPending]                 BIT              NOT NULL,
    [IsApproved]                BIT              NOT NULL,
    [DateSubmitted]             DATETIME         NOT NULL,
    [DateDecision]              DATETIME         NULL,
    [DecisionReason]            VARCHAR (MAX)    NULL,
    [ContactInformationRelease] BIT              NOT NULL
);







