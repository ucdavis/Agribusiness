declare @tmptable table (firstname varchar(50), lastname varchar(50), title varchar(200))

declare @cursor cursor
declare @spid int, @pid int, @firstname varchar(50), @lastname varchar(50), @title varchar(200), @year int

declare @contactid int = 0

set @cursor = cursor for
	select sp.id spid, sp.personid, firstname, lastname, s.[year]
	from seminarpeople sp
		inner join people p on sp.personid = p.id
		inner join seminars s on sp.seminarid = s.id
		
open @cursor

fetch next from @cursor into @spid, @pid, @firstname, @lastname, @year

while(@@FETCH_STATUS = 0)
begin

	print @spid
	print @pid
	print @firstname
	print @lastname
	print @year
		
	-- search for the title
	select @contactid = c_id
	from agribusinessarchive.dbo.seminars s
		inner join agribusinessarchive.dbo.contacts c on s.contactid = c.c_id
	where c.firstname = @firstname and c.lastname = @lastname
	  and s.[year] = @year

	-- contact was found
	if (@contactid > 0) 
	begin
		
		select @title = title
		from agribusinessarchive.dbo.contactfirms cf
		where cf.contactid = @contactid
		
		if (@title <> '')
		begin
			
			insert into @tmptable values (@firstname, @lastname, @title)
			
		end
		
		
	end
	
	fetch next from @cursor into @spid, @pid, @firstname, @lastname, @year

	set @contactid = 0
	set @title = ''

end

close @cursor
deallocate @cursor

select * from @tmptable