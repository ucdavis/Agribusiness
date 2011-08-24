ALTER TABLE [dbo].[MailingLists]
    ADD CONSTRAINT [DF_MailingLists_DateCreated] DEFAULT (getdate()) FOR [DateCreated];

