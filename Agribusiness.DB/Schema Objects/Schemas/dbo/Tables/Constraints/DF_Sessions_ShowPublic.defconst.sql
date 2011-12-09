ALTER TABLE [dbo].[Sessions]
    ADD CONSTRAINT [DF_Sessions_ShowPublic] DEFAULT ((1)) FOR [ShowPublic];

