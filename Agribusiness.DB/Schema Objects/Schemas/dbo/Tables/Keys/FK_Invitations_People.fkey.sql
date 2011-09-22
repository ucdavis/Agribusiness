ALTER TABLE [dbo].[Invitations]
    ADD CONSTRAINT [FK_Invitations_People] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[People] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

