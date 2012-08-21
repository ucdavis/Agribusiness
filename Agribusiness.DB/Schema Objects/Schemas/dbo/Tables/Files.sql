CREATE TABLE [dbo].[Files]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(100) NOT NULL, 
    [Contents] VARBINARY(MAX) NOT NULL, 
    [ContentType] VARCHAR(50) NOT NULL, 
    [FileName] VARCHAR(50) NOT NULL, 
    [SeminarId] INT NOT NULL, 
	[Public] BIT NOT NULL DEFAULT 0,
    [MySeminar] BIT NOT NULL DEFAULT 0, 
    [ProgramOverview] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_Files_Seminars] FOREIGN KEY ([SeminarId]) REFERENCES [Seminars]([Id])
)
