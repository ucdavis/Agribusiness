ALTER TABLE [dbo].[Applications]
    ADD CONSTRAINT [FK_Applications_FirmTypes] FOREIGN KEY ([FirmTypeId]) REFERENCES [dbo].[FirmTypes] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

