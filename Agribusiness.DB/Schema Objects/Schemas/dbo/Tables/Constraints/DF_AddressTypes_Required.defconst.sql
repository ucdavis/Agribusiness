ALTER TABLE [dbo].[AddressTypes]
    ADD CONSTRAINT [DF_AddressTypes_Required] DEFAULT ((0)) FOR [Required];

