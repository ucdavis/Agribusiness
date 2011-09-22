ALTER TABLE [dbo].[Invitations]
    ADD CONSTRAINT [FK_Invitations_Seminars] FOREIGN KEY ([SeminarId]) REFERENCES [dbo].[Seminars] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

