ALTER TABLE [dbo].[InformationRequests]
    ADD CONSTRAINT [FK_InformationRequests_Seminars] FOREIGN KEY ([SeminarId]) REFERENCES [dbo].[Seminars] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

