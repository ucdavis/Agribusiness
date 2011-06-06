ALTER TABLE [dbo].[Templates]
    ADD CONSTRAINT [DF_Templates_IsActive] DEFAULT ((1)) FOR [IsActive];

