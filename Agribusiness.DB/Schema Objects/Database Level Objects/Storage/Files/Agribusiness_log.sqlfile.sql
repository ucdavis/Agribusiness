﻿ALTER DATABASE [$(DatabaseName)]
    ADD LOG FILE (NAME = [Agribusiness_log], FILENAME = 'E:\DB\Agribusiness_log.ldf', SIZE = 11264 KB, MAXSIZE = 2097152 MB, FILEGROWTH = 10 %);
