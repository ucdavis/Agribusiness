ALTER TABLE [dbo].[SeminarPeopleXSessions]
    ADD CONSTRAINT [FK_SeminarPeopleSessions_Sessions] FOREIGN KEY ([SessionId]) REFERENCES [dbo].[Sessions] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

