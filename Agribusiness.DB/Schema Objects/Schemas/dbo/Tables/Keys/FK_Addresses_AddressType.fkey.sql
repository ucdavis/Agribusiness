ALTER TABLE [dbo].[Addresses]
    ADD CONSTRAINT [FK_Addresses_AddressType] FOREIGN KEY ([AddressTypeId]) REFERENCES [dbo].[AddressTypes] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

