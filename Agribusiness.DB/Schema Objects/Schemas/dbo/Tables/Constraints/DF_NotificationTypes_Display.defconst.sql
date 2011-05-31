ALTER TABLE [dbo].[NotificationTypes]
    ADD CONSTRAINT [DF_NotificationTypes_Display] DEFAULT ((1)) FOR [Display];

