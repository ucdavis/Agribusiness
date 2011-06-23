ALTER TABLE [dbo].[InformationRequests]
    ADD CONSTRAINT [DF_InformationRequests_SubmittedDateTime] DEFAULT (getdate()) FOR [SubmittedDateTime];

