CREATE TABLE [dbo].[Contacts] (
    [id]            INT          IDENTITY (1, 1) NOT NULL,
    [FirstName]     VARCHAR (50) NOT NULL,
    [LastName]      VARCHAR (50) NOT NULL,
    [Phone]         VARCHAR (15) NOT NULL,
    [Email]         VARCHAR (50) NULL,
    [ContactTypeId] CHAR (1)     NOT NULL,
    [PersonId]      INT          NOT NULL,
    [Ext]           VARCHAR (10) NULL
);



