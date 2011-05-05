CREATE TABLE [dbo].[CommunicationOptions] (
    [Id]                CHAR (2)      NOT NULL,
    [Name]              VARCHAR (50)  NOT NULL,
    [Description]       VARCHAR (100) NULL,
    [RequiresAssistant] BIT           NOT NULL
);

