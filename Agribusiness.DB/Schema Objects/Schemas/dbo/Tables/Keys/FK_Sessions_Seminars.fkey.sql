ALTER TABLE [dbo].[Sessions]
    ADD CONSTRAINT [FK_Sessions_Seminars] FOREIGN KEY ([SeminarId]) REFERENCES [dbo].[Seminars] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

