ALTER TABLE [dbo].[EmailQueue]
    ADD CONSTRAINT [FK_EmailQueue_People] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[People] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

