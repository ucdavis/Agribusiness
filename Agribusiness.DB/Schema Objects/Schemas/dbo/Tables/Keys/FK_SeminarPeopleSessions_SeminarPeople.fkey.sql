ALTER TABLE [dbo].[SeminarPeopleXSessions]
    ADD CONSTRAINT [FK_SeminarPeopleSessions_SeminarPeople] FOREIGN KEY ([SeminarPersonId]) REFERENCES [dbo].[SeminarPeople] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

