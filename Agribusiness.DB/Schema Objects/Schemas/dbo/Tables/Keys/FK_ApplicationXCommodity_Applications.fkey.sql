ALTER TABLE [dbo].[ApplicationXCommodity]
    ADD CONSTRAINT [FK_ApplicationXCommodity_Applications] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[Applications] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

