ALTER TABLE [dbo].[NotificationTracking]
    ADD CONSTRAINT [FK_NotificationTracking_NotificationMethods] FOREIGN KEY ([NotificationMethodId]) REFERENCES [dbo].[NotificationMethods] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

