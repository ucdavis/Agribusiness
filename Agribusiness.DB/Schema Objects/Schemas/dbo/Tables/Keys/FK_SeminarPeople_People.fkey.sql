ALTER TABLE [dbo].[SeminarPeople]
    ADD CONSTRAINT [FK_SeminarPeople_People] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[People] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

