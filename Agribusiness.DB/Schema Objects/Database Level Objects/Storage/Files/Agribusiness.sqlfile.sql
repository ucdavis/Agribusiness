﻿ALTER DATABASE [$(DatabaseName)]
    ADD FILE (NAME = [Agribusiness], FILENAME = 'c:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLEXPRESS\MSSQL\DATA\Agribusiness.mdf', SIZE = 2048 KB, FILEGROWTH = 1024 KB) TO FILEGROUP [PRIMARY];



