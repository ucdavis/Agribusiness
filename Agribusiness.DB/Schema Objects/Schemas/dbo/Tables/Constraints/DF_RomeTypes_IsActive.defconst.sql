ALTER TABLE [dbo].[RoomTypes]
    ADD CONSTRAINT [DF_RomeTypes_IsActive] DEFAULT ((1)) FOR [IsActive];

