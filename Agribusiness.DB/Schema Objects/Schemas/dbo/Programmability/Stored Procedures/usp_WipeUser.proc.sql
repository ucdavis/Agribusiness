-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE usp_WipeUser

	@userid uniqueidentifier

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	delete from seminarpeoplexsessions where seminarpersonid in (select id from seminarpeople where personid in(select id from people where userid = @userid))
	delete from seminarpeople where personid in(select id from people where userid = @userid)
	delete from people where userid = @userid

	delete from applicationxcommodity where applicationid in ( select id from applications where userid = @userid )
	delete from applications where userid = @userid

	delete from aspnet_membership where userid = @userid
	delete from aspnet_users where userid = @userid
END