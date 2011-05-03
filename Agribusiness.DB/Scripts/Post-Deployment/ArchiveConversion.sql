--------------------------
-- Get the distinct years
--------------------------
/*
insert into agribusiness.dbo.seminars (year, location, [begin], [end], releasetoattendees)
select distinct [YEAR], 'n/a' location
	, CAST(cast([year] as varchar(4)) + '-1-1' as datetime) [begin]
	, CAST(cast([year] as varchar(4)) + '-1-2' as datetime) [end]
	, 0 releaseToAttendees
from agribusinessarchive.dbo.seminars order by year
*/

--------------------------
-- Get the list of contacts
--------------------------

/*
alter table agribusiness.dbo.people
add archive_id uniqueidentifier null
go

-- conversion between id's
create view vContacts as
select p.id personId, c.id contactId
from agribusiness.dbo.people p
	inner join agribusinessarchive.dbo.contacts c on p.archive_id = c.id
go

declare @id uniqueidentifier, @lastname nvarchar(max), @firstname nvarchar(max), @mi nvarchar(max)
	  , @salutation nvarchar(max), @badge nvarchar(max), @phone nvarchar(max), @cell nvarchar(max)
	  , @fax nvarchar(max), @biography nvarchar(max), @email nvarchar(max)
	  , @password nvarchar(max), @userId uniqueidentifier, @return_value int, @date datetime
	  
set @date = getdate()

declare @contactCursor cursor 
set @contactCursor = cursor for
	select id, lastname, firstname, mi, salutation, badge, isnull(phone, 'n/a') phone, cellphone, fax, biography
		 , isnull(email, firstname+'.'+lastname+'@fake.com') email, isnull(password, 'password') password
	from contacts

open @contactCursor

fetch next from @contactCursor into @id, @lastname, @firstname, @mi, @salutation, @badge, @phone, @cell, @fax, @biography, @email, @password

while(@@FETCH_STATUS = 0)
begin

	-- create the user
	set @userid = null
	
	EXEC	@return_value = [Agribusiness].[dbo].[aspnet_Membership_CreateUser]
		@ApplicationName = N'Agribusiness',
		@UserName = @email,
		@Password = @password,
		@PasswordSalt = N'salt',
		@Email = @email,
		@PasswordQuestion = N'Q',
		@PasswordAnswer = N'A',
		@IsApproved = 1,
		@CurrentTimeUtc = @date,
		@CreateDate = @date,
		@UserId = @UserId OUTPUT

	
	if (@userid is null) begin
		print @return_value
		print @password
		print @email
	end

	insert into agribusiness.dbo.people (archive_id, lastname, firstname, mi, salutation, badgename, phone, cellphone, fax, biography, userid)
	values (@id, @lastname, @firstname, @mi, @salutation, @badge, @phone, @cell, @fax, @biography, @userid)
	
	fetch next from @contactCursor into @id, @lastname, @firstname, @mi, @salutation, @badge, @phone, @cell, @fax, @biography, @email, @password
end

close @contactCursor
deallocate @contactCursor
*/

--------------------------
-- Addresses
--------------------------

/*
insert into agribusiness.dbo.addresses(personId, city, country, state, line1, zip, addresstypeid)
select personId, isnull(couriercity, 'n/a') city, couriercountry, isnull(courierstate, '') state, isnull(courierstreet, 'n/a') address, isnull(courierzip, 'n/a') zip, 'C'
from agribusinessarchive.dbo.contacts c
	inner join agribusinessarchive.dbo.vContacts vc on vc.contactId = c.id
where courierstreet is not null
  and (couriercountry = 'USA' or couriercountry is null)


insert into agribusiness.dbo.addresses(personid, line1, city, country, state, zip, addresstypeid)
select vc.personId, isnull(f.address, 'n/a') address, isnull(f.city, 'n/a') city, f.country, isnull(f.state, ''), isnull(f.zip, 'n/a') zip, 'B'
from agribusinessArchive.dbo.contactfirms
	inner join agribusinessArchive.dbo.firms f on f.f_id = contactfirms.firmid
	inner join agribusinessArchive.dbo.contacts c on c.c_id = contactfirms.contactid
	inner join agribusinessArchive.dbo.vContacts vc on vc.contactId = c.id
where country = 'USA' or country is null
*/

--------------------------
-- Get the contacts
--------------------------

/*
insert into agribusiness.dbo.contacts (firstname, lastname, email, phone, ext, contacttypeid, personid)
select assistantname, 'n/a', substring(isnull(assistantemail, 'n/a'), 1, 50)
	 , substring(isnull(assistantphone, 'n/a'), 1, 15)
	 , substring(assistantExt, 1, 10), 'A', vc.personId
from agribusinessarchive.dbo.contacts 
	inner join agribusinessarchive.dbo.vcontacts vc on contacts.id = vc.contactId
where assistantname is not null

insert into agribusiness.dbo.contacts (firstname, lastname, phone, ext, contacttypeid, personid)
select emergencyname, 'n/a'
	, substring(isnull(emergencyphone, 'n/a'), 1, 15)
	, substring(isnull(emergencyext, 'n/a'), 1, 10), 'E', vc.personId
from agribusinessarchive.dbo.contacts 
	inner join agribusinessarchive.dbo.vcontacts vc on contacts.id = vc.contactId
where emergencyname is not null
*/

--------------------------
-- Get the seminar people
--------------------------

