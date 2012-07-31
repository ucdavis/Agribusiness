﻿CREATE TABLE [dbo].[InformationRequests] (
    [Id]                INT           IDENTITY (1, 1) NOT NULL,
    [FirstName]              VARCHAR (50) NOT NULL,
	[LastName]              VARCHAR (50) NOT NULL,
    [Title]             VARCHAR (75)  NOT NULL,
    [Company]           VARCHAR (75)  NOT NULL,
    [Email]             VARCHAR (50)  NOT NULL,
    [Commodity]         VARCHAR (200) NOT NULL,
    [SeminarId]         INT           NOT NULL,
    [SubmittedDateTime] DATETIME      NOT NULL,
    [Responded]         BIT           NOT NULL, 
    [SiteId] VARCHAR(10) NOT NULL, 
    [AddressLine1]          VARCHAR (100)    NOT NULL,
    [AddressLine2]          VARCHAR (100)    NULL,
    [City]                  VARCHAR (50)     NOT NULL,
    [State]                 VARCHAR (50)     NOT NULL,
    [Zip]                   VARCHAR (10)     NOT NULL,
    [Country]               CHAR (3)         NOT NULL,
    [ReferredBy] VARCHAR(50) NULL, 
    [AssistantFirstName] VARCHAR(50) NULL, 
    [AssistantLastName] VARCHAR(50) NULL, 
    [AssistantEmail] VARCHAR(50) NULL, 
    [AssistantPhone] VARCHAR(20) NULL, 
    CONSTRAINT [FK_InformationRequests_Sites] FOREIGN KEY ([SiteId]) REFERENCES [Sites]([Id])
);

