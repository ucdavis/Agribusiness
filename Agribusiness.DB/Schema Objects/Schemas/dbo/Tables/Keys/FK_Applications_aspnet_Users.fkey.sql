ALTER TABLE [dbo].[Applications]
    ADD CONSTRAINT [FK_Applications_aspnet_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[aspnet_Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

