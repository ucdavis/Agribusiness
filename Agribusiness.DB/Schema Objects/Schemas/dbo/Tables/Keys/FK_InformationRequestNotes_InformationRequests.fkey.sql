ALTER TABLE [dbo].[InformationRequestNotes]
    ADD CONSTRAINT [FK_InformationRequestNotes_InformationRequests] FOREIGN KEY ([InformationRequestId]) REFERENCES [dbo].[InformationRequests] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

