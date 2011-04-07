ALTER TABLE [dbo].[Applications]
    ADD CONSTRAINT [FK_Applications_Seminars] FOREIGN KEY ([SeminarId]) REFERENCES [dbo].[Seminars] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

