ALTER TABLE [dbo].[CaseStudyExecutives]
    ADD CONSTRAINT [FK_CaseStudyExecutives_CaseStudies] FOREIGN KEY ([CaseStudyId]) REFERENCES [dbo].[CaseStudies] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

