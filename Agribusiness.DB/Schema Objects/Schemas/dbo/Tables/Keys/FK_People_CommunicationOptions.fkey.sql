ALTER TABLE [dbo].[People]
    ADD CONSTRAINT [FK_People_CommunicationOptions] FOREIGN KEY ([CommunicationOptionId]) REFERENCES [dbo].[CommunicationOptions] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

