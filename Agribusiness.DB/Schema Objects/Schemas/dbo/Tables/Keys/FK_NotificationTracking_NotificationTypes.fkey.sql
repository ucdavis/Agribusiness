ALTER TABLE [dbo].[NotificationTracking]
    ADD CONSTRAINT [FK_NotificationTracking_NotificationTypes] FOREIGN KEY ([NotificationTypeId]) REFERENCES [dbo].[NotificationTypes] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

