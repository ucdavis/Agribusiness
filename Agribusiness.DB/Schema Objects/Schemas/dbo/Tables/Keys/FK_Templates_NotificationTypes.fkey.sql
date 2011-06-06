ALTER TABLE [dbo].[Templates]
    ADD CONSTRAINT [FK_Templates_NotificationTypes] FOREIGN KEY ([NotificationTypeId]) REFERENCES [dbo].[NotificationTypes] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

