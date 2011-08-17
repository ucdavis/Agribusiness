ALTER TABLE [dbo].[CaseStudies]
    ADD CONSTRAINT [DF_CaseStudies_Public] DEFAULT ((0)) FOR [IsPublic];

