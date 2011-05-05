ALTER TABLE [dbo].[CommunicationOptions]
    ADD CONSTRAINT [DF_CommunicationOptions_RequiresAssistant] DEFAULT ((0)) FOR [RequiresAssistant];

