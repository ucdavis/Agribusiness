ALTER DATABASE [$(DatabaseName)]
    ADD LOG FILE (NAME = [Agribusiness_log], FILENAME = 'c:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLEXPRESS\MSSQL\DATA\AgribusinessI.ldf', SIZE = 1024 KB, MAXSIZE = 2097152 MB, FILEGROWTH = 10 %);





