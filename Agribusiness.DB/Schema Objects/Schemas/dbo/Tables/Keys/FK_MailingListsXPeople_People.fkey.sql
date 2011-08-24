ALTER TABLE [dbo].[MailingListsXPeople]
    ADD CONSTRAINT [FK_MailingListsXPeople_People] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[People] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

