ALTER TABLE [dbo].[CaseStudies]
    ADD CONSTRAINT [FK_CaseStudies_Seminars] FOREIGN KEY ([SeminarId]) REFERENCES [dbo].[Seminars] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

