ALTER TABLE [dbo].[NotificationTypes]
    ADD CONSTRAINT [DF_NotificationTypes_SendAll] DEFAULT ((0)) FOR [SendAll];

