ALTER TABLE [dbo].[People]
    ADD CONSTRAINT [DF_People_AutomatedNotification] DEFAULT ((1)) FOR [AutomatedNotification];

