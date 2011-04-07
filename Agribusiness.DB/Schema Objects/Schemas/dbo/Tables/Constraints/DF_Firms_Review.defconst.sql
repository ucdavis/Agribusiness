ALTER TABLE [dbo].[Firms]
    ADD CONSTRAINT [DF_Firms_Review] DEFAULT ((1)) FOR [Review];

