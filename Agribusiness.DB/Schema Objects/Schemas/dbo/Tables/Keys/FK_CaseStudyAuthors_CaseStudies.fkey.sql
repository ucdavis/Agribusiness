ALTER TABLE [dbo].[CaseStudyAuthors]
    ADD CONSTRAINT [FK_CaseStudyAuthors_CaseStudies] FOREIGN KEY ([CaseStudyId]) REFERENCES [dbo].[CaseStudies] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

