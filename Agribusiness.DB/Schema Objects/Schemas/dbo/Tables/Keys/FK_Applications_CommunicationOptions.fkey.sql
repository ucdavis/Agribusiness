ALTER TABLE [dbo].[Applications]
    ADD CONSTRAINT [FK_Applications_CommunicationOptions] FOREIGN KEY ([CommunicationOptionId]) REFERENCES [dbo].[CommunicationOptions] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

