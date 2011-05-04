ALTER TABLE [dbo].[Applications]
    ADD CONSTRAINT [FK_Applications_Countries] FOREIGN KEY ([FirmCountry]) REFERENCES [dbo].[Countries] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

