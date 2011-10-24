-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION IsEmail
(
	@email varchar(100)
)
RETURNS bit
AS
BEGIN

	IF (
		 CHARINDEX(' ',LTRIM(RTRIM(@email))) = 0 
	AND  LEFT(LTRIM(@email),1) <> '@' 
	AND  RIGHT(RTRIM(@email),1) <> '.' 
	AND  CHARINDEX('.',@email ,CHARINDEX('@',@email)) - CHARINDEX('@',@email ) > 1 
	AND  LEN(LTRIM(RTRIM(@email ))) - LEN(REPLACE(LTRIM(RTRIM(@email)),'@','')) = 1 
	AND  CHARINDEX('.',REVERSE(LTRIM(RTRIM(@email)))) >= 3 
	AND  (CHARINDEX('.@',@email ) = 0 AND CHARINDEX('..',@email ) = 0)
	)
	   return 1
	
	return 0

END