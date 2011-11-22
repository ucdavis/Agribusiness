CREATE TABLE [dbo].[Attachments]
(
	[Id]			int				IDENTITY(1,1) NOT NULL, 
	[Contents]		varbinary(max)	NOT NULL,
	[FileName]		varchar(100)	NOT NULL,
	[ContentType]	varchar(50)		NOT NULL
)
