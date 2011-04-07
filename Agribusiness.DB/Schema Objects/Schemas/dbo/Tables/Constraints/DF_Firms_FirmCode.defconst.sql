ALTER TABLE [dbo].[Firms]
    ADD CONSTRAINT [DF_Firms_FirmCode] DEFAULT (newid()) FOR [FirmCode];

