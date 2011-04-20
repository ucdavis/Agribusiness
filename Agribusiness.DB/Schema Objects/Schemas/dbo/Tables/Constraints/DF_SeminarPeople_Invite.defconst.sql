ALTER TABLE [dbo].[SeminarPeople]
    ADD CONSTRAINT [DF_SeminarPeople_Invite] DEFAULT ((0)) FOR [Invite];

