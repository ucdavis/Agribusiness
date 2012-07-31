CREATE TABLE [dbo].[PeopleXSites]
(
	[PersonId] int NOT NULL,
	SiteId varchar(10) NOT NULL, 
    CONSTRAINT [PK_PeopleXSites] PRIMARY KEY ([PersonId], [SiteId]), 
    CONSTRAINT [FK_PeopleXSites_People] FOREIGN KEY ([PersonId]) REFERENCES [People]([Id]), 
    CONSTRAINT [FK_PeopleXSites_Sites] FOREIGN KEY ([SiteId]) REFERENCES [Sites]([Id])

)
