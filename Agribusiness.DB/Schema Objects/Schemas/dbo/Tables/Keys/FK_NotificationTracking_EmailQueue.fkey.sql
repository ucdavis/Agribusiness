ALTER TABLE [dbo].[NotificationTracking]
    ADD CONSTRAINT [FK_NotificationTracking_EmailQueue] FOREIGN KEY ([EmailQueueId]) REFERENCES [dbo].[EmailQueue] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

