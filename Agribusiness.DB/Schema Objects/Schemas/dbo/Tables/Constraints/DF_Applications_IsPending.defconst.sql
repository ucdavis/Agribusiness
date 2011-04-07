ALTER TABLE [dbo].[Applications]
    ADD CONSTRAINT [DF_Applications_IsPending] DEFAULT ((1)) FOR [IsPending];

