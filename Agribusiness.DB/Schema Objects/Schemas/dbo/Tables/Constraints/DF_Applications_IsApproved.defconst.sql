ALTER TABLE [dbo].[Applications]
    ADD CONSTRAINT [DF_Applications_IsApproved] DEFAULT ((0)) FOR [IsApproved];

