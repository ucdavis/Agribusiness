CREATE PROCEDURE [dbo].[usp_SendEmails]
@test BIT, @email NVARCHAR (4000)
AS EXTERNAL NAME [Agribusiness.CLR].[StoredProcedures].[usp_SendEmails]



