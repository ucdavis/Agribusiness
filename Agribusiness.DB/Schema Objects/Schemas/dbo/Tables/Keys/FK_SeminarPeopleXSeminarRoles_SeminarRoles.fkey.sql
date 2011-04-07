ALTER TABLE [dbo].[SeminarPeopleXSeminarRoles]
    ADD CONSTRAINT [FK_SeminarPeopleXSeminarRoles_SeminarRoles] FOREIGN KEY ([SeminarRoleId]) REFERENCES [dbo].[SeminarRoles] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

