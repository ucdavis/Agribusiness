ALTER TABLE [dbo].[SeminarPeople]
    ADD CONSTRAINT [DF_SeminarPeople_ContactInformationRelease] DEFAULT ((0)) FOR [ContactInformationRelease];

