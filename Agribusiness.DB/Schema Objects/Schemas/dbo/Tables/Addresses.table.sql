CREATE TABLE [dbo].[Addresses] (
    [id]            INT           IDENTITY (1, 1) NOT NULL,
    [Line1]         VARCHAR (100) NOT NULL,
    [Line2]         VARCHAR (100) NULL,
    [City]          VARCHAR (50)  NOT NULL,
    [State]         CHAR (2)      NOT NULL,
    [Zip]           VARCHAR (10)  NOT NULL,
    [AddressTypeId] CHAR (1)      NOT NULL,
    [PersonId]      INT           NOT NULL,
    [Country]       VARCHAR (50)  NULL
);



