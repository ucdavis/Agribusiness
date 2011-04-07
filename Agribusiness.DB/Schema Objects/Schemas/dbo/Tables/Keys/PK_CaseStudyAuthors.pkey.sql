﻿ALTER TABLE [dbo].[CaseStudyAuthors]
    ADD CONSTRAINT [PK_CaseStudyAuthors] PRIMARY KEY CLUSTERED ([CaseStudyId] ASC, [SeminarPersonId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

