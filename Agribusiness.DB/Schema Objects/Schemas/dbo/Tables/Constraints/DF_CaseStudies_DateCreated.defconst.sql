ALTER TABLE [dbo].[CaseStudies]
    ADD CONSTRAINT [DF_CaseStudies_DateCreated] DEFAULT (getdate()) FOR [DateCreated];

