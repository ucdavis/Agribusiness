ALTER TABLE [dbo].[People]
    ADD CONSTRAINT [DF_People_ContactInformationRelease] DEFAULT ((0)) FOR [ContactInformationRelease];

