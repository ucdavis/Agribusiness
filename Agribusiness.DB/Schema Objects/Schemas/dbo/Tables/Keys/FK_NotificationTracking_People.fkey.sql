ALTER TABLE [dbo].[NotificationTracking]
    ADD CONSTRAINT [FK_NotificationTracking_People] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[People] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

