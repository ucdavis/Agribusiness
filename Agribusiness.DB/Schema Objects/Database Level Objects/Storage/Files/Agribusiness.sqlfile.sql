ALTER DATABASE [$(DatabaseName)]
    ADD FILE (NAME = [Agribusiness], FILENAME = 'E:\DB\Agribusiness.mdf', SIZE = 3072 KB, FILEGROWTH = 1024 KB) TO FILEGROUP [PRIMARY];

