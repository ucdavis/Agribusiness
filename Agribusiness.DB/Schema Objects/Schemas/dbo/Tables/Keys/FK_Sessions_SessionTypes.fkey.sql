ALTER TABLE [dbo].[Sessions]
    ADD CONSTRAINT [FK_Sessions_SessionTypes] FOREIGN KEY ([SessionTypeId]) REFERENCES [dbo].[SessionTypes] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

