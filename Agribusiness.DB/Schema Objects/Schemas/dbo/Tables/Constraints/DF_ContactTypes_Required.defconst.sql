ALTER TABLE [dbo].[ContactTypes]
    ADD CONSTRAINT [DF_ContactTypes_Required] DEFAULT ((0)) FOR [Required];

