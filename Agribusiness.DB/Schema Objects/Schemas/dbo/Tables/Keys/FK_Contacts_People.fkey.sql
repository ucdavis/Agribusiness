ALTER TABLE [dbo].[Contacts]
    ADD CONSTRAINT [FK_Contacts_People] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[People] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

