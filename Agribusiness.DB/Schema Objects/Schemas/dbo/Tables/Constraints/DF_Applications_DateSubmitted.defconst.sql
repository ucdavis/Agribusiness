ALTER TABLE [dbo].[Applications]
    ADD CONSTRAINT [DF_Applications_DateSubmitted] DEFAULT (getdate()) FOR [DateSubmitted];

