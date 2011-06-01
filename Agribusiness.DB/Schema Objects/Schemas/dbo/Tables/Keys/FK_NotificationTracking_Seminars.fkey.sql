ALTER TABLE [dbo].[NotificationTracking]
    ADD CONSTRAINT [FK_NotificationTracking_Seminars] FOREIGN KEY ([SeminarId]) REFERENCES [dbo].[Seminars] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

