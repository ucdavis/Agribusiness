ALTER TABLE [dbo].[ApplicationXCommodity]
    ADD CONSTRAINT [FK_ApplicationXCommodity_Commodities] FOREIGN KEY ([CommodityId]) REFERENCES [dbo].[Commodities] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

