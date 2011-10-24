CREATE PROCEDURE [dbo].[usp_ProcessEmails]

AS

declare @cursor cursor

declare @id int, @subject varchar(100), @body varchar(max), @from varchar(50), @email varchar(max)

set @cursor = cursor for
	select id, subject, body, fromaddress, email from vEmailQueue
	
open @cursor

fetch next from @cursor into @id, @subject, @body, @from, @email

while(@@FETCH_STATUS = 0)
begin

	exec msdb.dbo.sp_send_dbmail
		@profile_name = 'Agribusiness',
		@from_address = @from,
		@reply_to = 'agribusiness@ucdavis.edu',
		@recipients = @email,
		@subject = @subject,
		@body = @body,
		@body_format= 'HTML'

	update emailqueue set pending = 0, SentDateTime = getdate() where id = @id

	fetch next from @cursor into @id, @subject, @body, @from, @email

end

close @cursor
deallocate @cursor



RETURN 0