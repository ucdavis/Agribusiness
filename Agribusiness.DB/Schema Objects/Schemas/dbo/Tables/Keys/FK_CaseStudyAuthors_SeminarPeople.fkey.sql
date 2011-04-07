ALTER TABLE [dbo].[CaseStudyAuthors]
    ADD CONSTRAINT [FK_CaseStudyAuthors_SeminarPeople] FOREIGN KEY ([SeminarPersonId]) REFERENCES [dbo].[SeminarPeople] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

