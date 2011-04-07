ALTER TABLE [dbo].[SeminarPeople]
    ADD CONSTRAINT [FK_SeminarPeople_Seminars] FOREIGN KEY ([SeminarId]) REFERENCES [dbo].[Seminars] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

