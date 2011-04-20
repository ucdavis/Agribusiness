ALTER TABLE [dbo].[Seminars]
    ADD CONSTRAINT [DF_Seminars_ReleaseToAttendees] DEFAULT ((0)) FOR [ReleaseToAttendees];

