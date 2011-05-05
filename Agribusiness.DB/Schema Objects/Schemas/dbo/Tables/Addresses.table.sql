CREATE TABLE [dbo].[Addresses] (
    [id]            INT           IDENTITY (1, 1) NOT NULL,
    [Line1]         VARCHAR (200) NOT NULL,
    [Line2]         VARCHAR (200) NULL,
    [City]          VARCHAR (50)  NOT NULL,
    [State]         VARCHAR (50)  NOT NULL,
    [Zip]           VARCHAR (15)  NOT NULL,
    [AddressTypeId] CHAR (1)      NOT NULL,
    [PersonId]      INT           NOT NULL,
    [CountryId]     CHAR (3)      NULL,
    [Description]   VARCHAR (200) NULL
);











