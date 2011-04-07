ALTER TABLE [dbo].[People]
    ADD CONSTRAINT [DF_People_Invite] DEFAULT ((0)) FOR [Invite];

