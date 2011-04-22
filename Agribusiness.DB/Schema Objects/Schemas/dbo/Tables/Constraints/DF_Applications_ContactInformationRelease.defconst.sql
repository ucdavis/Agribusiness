ALTER TABLE [dbo].[Applications]
    ADD CONSTRAINT [DF_Applications_ContactInformationRelease] DEFAULT ((0)) FOR [ContactInformationRelease];

