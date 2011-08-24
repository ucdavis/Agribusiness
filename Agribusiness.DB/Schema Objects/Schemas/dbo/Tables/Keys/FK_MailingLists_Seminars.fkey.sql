ALTER TABLE [dbo].[MailingLists]
    ADD CONSTRAINT [FK_MailingLists_Seminars] FOREIGN KEY ([SeminarId]) REFERENCES [dbo].[Seminars] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

