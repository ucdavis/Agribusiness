ALTER TABLE [dbo].[NotificationTracking]
    ADD CONSTRAINT [FK_NotificationTracking_Applications] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[Applications] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

