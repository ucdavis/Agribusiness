ALTER TABLE [dbo].[SeminarPeopleXSeminarRoles]
    ADD CONSTRAINT [FK_SeminarPeopleXSeminarRoles_SeminarPeople] FOREIGN KEY ([SeminarPersonId]) REFERENCES [dbo].[SeminarPeople] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

