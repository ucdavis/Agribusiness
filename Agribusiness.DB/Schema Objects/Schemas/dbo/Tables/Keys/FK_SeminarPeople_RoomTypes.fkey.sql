ALTER TABLE [dbo].[SeminarPeople]
    ADD CONSTRAINT [FK_SeminarPeople_RoomTypes] FOREIGN KEY ([RoomTypeId]) REFERENCES [dbo].[RoomTypes] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

