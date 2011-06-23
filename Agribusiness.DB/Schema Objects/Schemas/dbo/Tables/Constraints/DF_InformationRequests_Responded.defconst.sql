ALTER TABLE [dbo].[InformationRequests]
    ADD CONSTRAINT [DF_InformationRequests_Responded] DEFAULT ((0)) FOR [Responded];

