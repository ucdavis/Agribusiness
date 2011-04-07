ALTER TABLE [dbo].[Contacts]
    ADD CONSTRAINT [FK_Contacts_ContactType] FOREIGN KEY ([ContactTypeId]) REFERENCES [dbo].[ContactTypes] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

