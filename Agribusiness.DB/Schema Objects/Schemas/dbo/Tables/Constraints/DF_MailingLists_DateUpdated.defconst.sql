ALTER TABLE [dbo].[MailingLists]
    ADD CONSTRAINT [DF_MailingLists_DateUpdated] DEFAULT (getdate()) FOR [DateUpdated];

