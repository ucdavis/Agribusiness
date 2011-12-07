ALTER TABLE [dbo].[FirmTypes]
    ADD CONSTRAINT [DF_FirmTypes_IsActive] DEFAULT ((1)) FOR [IsActive];

