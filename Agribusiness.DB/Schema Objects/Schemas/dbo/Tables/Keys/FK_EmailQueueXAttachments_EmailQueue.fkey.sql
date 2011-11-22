ALTER TABLE [dbo].[EmailQueueXAttachments]
    ADD CONSTRAINT [FK_EmailQueueXAttachments_EmailQueue] FOREIGN KEY ([EmailQueueId]) REFERENCES [dbo].[EmailQueue] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

