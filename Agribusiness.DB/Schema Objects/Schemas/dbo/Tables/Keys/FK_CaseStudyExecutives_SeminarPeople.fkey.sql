ALTER TABLE [dbo].[CaseStudyExecutives]
    ADD CONSTRAINT [FK_CaseStudyExecutives_SeminarPeople] FOREIGN KEY ([SeminarPersonId]) REFERENCES [dbo].[SeminarPeople] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

