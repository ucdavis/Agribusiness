ALTER TABLE [dbo].[Applications]
    ADD CONSTRAINT [FK_Applications_States] FOREIGN KEY ([FirmState]) REFERENCES [dbo].[States] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

