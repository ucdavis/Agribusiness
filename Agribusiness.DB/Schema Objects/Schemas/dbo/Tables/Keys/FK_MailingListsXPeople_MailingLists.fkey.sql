ALTER TABLE [dbo].[MailingListsXPeople]
    ADD CONSTRAINT [FK_MailingListsXPeople_MailingLists] FOREIGN KEY ([MailingListId]) REFERENCES [dbo].[MailingLists] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

