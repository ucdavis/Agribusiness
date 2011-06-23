ALTER TABLE [dbo].[InformationRequestNotes]
    ADD CONSTRAINT [DF_InformationRequestNotes_DateTime] DEFAULT (getdate()) FOR [DateTimeNote];

