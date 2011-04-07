ALTER TABLE [dbo].[CaseStudies]
    ADD CONSTRAINT [FK_CaseStudies_Sessions] FOREIGN KEY ([SessionId]) REFERENCES [dbo].[Sessions] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

