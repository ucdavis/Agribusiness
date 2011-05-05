ALTER TABLE [dbo].[Addresses]
    ADD CONSTRAINT [FK_Addresses_Countries] FOREIGN KEY ([CountryId]) REFERENCES [dbo].[Countries] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

