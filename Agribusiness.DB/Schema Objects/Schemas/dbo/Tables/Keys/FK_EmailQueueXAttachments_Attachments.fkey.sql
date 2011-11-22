ALTER TABLE [dbo].[EmailQueueXAttachments]
    ADD CONSTRAINT [FK_EmailQueueXAttachments_Attachments] FOREIGN KEY ([AttachmentId]) REFERENCES [dbo].[Attachments] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

