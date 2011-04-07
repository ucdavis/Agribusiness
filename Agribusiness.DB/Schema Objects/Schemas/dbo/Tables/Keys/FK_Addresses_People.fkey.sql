ALTER TABLE [dbo].[Addresses]
    ADD CONSTRAINT [FK_Addresses_People] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[People] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

