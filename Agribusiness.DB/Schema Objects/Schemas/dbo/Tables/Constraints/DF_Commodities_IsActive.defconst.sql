ALTER TABLE [dbo].[Commodities]
    ADD CONSTRAINT [DF_Commodities_IsActive] DEFAULT ((1)) FOR [IsActive];

