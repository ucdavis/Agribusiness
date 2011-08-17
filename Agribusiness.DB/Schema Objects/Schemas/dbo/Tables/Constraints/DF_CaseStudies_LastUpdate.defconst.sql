ALTER TABLE [dbo].[CaseStudies]
    ADD CONSTRAINT [DF_CaseStudies_LastUpdate] DEFAULT (getdate()) FOR [LastUpdate];

