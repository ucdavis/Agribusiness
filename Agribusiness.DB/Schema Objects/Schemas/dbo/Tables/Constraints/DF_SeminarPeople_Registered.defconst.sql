ALTER TABLE [dbo].[SeminarPeople]
    ADD CONSTRAINT [DF_SeminarPeople_Registered] DEFAULT ((0)) FOR [Paid];



