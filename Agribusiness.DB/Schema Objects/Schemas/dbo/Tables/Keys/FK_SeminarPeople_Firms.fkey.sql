ALTER TABLE [dbo].[SeminarPeople]
    ADD CONSTRAINT [FK_SeminarPeople_Firms] FOREIGN KEY ([FirmId]) REFERENCES [dbo].[Firms] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

