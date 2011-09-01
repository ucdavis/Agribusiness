--select distinct *
--from contacts
--where c_id in (
--	select contactid
--	from seminars s
--	where contactid not in (
--		select c.c_id
--		from contacts c
--		inner join agribusiness.dbo.people p on c.firstname = p.firstname and c.lastname = p.lastname
--	)
--	and contactid in ( select c_id from contacts )
--	and (
--		iscaseexecutive = 0
--		and
--		isdiscussiongrouplead = 0
--		and
--		isfaculty = 0
--		and
--		issteeringcommittee = 0
--		and 
--		ispanelist = 0
--		and
--		isparticipant = 0
--		and
--		isspeaker = 0
--		and
--		isstaff = 0
--		and 
--		isvendor = 0
--	)
--)

declare @id uniqueidentifier, @lastname nvarchar(max), @firstname nvarchar(max), @mi nvarchar(max)
	  , @salutation nvarchar(max), @badge nvarchar(max), @phone nvarchar(max), @cell nvarchar(max)
	  , @fax nvarchar(max), @biography nvarchar(max), @email nvarchar(max)
	  , @password nvarchar(max)
	  
declare @userId uniqueidentifier, @return_value int, @date datetime = getdate()
	  
declare @cstreet varchar(max), @ccity varchar(max), @cstate varchar(max), @czip varchar(max), @ccountry varchar(max)
declare @bstreet varchar(max), @bcity varchar(max), @bstate varchar(max), @bzip varchar(max), @bcountry varchar(max)

declare @personid int

declare @contactcursor cursor

set @contactcursor = cursor for
	select distinct contacts.id, lastname, firstname, mi, salutation, badge
			 , substring(isnull(contacts.phone, 'n/a'), 1, 15) phone, cellphone, contacts.fax, biography
			 , isnull(email, firstname+'.'+lastname+'@fake.com') email, isnull(password, 'password') password
			 
			 , isnull(couriercity, 'n/a') city
			 , isnull(isnull(countries.id, couriercountry), 'USA') country
			 , isnull(courierstate, 'n/a') state
			 , isnull(courierstreet, 'n/a') address, isnull(courierzip, 'n/a') zip
			 
			 , isnull(f.city, 'n/a') city
			 , isnull(isnull(countries.id, f.country), 'USA') country
			 , isnull(f.state, '')
			 , isnull(f.address, 'n/a') address, isnull(f.zip, 'n/a') zip
			 
		from contacts
			left outer join contactfirms cf on contacts.c_id = cf.contactid
			left outer join firms f on f.f_id = cf.firmid
			left outer join agribusiness.dbo.countries on f.country = countries.name
		where c_id in (
			select contactid
			from seminars s
			where contactid not in (
				select c.c_id
				from contacts c
				inner join agribusiness.dbo.people p on c.firstname = p.firstname and c.lastname = p.lastname
			)
			and contactid in ( select c_id from contacts )
			and ( iscaseexecutive = 0 and isdiscussiongrouplead = 0 and isfaculty = 0 and issteeringcommittee = 0 and 
				ispanelist = 0 and isparticipant = 0 and isspeaker = 0 and isstaff = 0 and isvendor = 0)
		)
		
open @contactcursor

fetch next from @contactcursor into @id, @lastname, @firstname, @mi, @salutation, @badge, @phone, @cell, @fax, @biography, @email, @password
								  , @ccity, @ccountry, @cstate, @cstreet, @czip
								  , @bcity, @bcountry, @bstate, @bstreet, @bzip
								  
while (@@FETCH_STATUS = 0)
begin

	-- create the user
	set @userid = null

	select top 1 @userid = userid from agribusiness.dbo.aspnet_users where username = @email

	--EXEC	@return_value = [Agribusiness].[dbo].[aspnet_Membership_CreateUser]
	--	@ApplicationName = N'Agribusiness',
	--	@UserName = @email,
	--	@Password = @password,
	--	@PasswordSalt = N'salt',
	--	@Email = @email,
	--	@PasswordQuestion = N'Q',
	--	@PasswordAnswer = N'A',
	--	@IsApproved = 1,
	--	@CurrentTimeUtc = @date,
	--	@CreateDate = @date,
	--	@UserId = @UserId OUTPUT

	
	--if (@userid is null) begin
	--	print @return_value
	--	print @password
	--	print @email
	--end

	--print @id

	-- inesrt into the person object
	insert into agribusiness.dbo.people (lastname, firstname, mi, salutation, badgename, phone, cellphone, fax, biography, userid, communicationoptionid, automatednotification)
	values (@lastname, @firstname, @mi, @salutation, @badge, @phone, @cell, @fax, @biography, @userid, 'DR', 1)

	-- add in the addresses
	select top 1 @personid = id from agribusiness.dbo.people where userid = @userid

	insert into agribusiness.dbo.addresses	(line1, city, state, zip, addresstypeid, personid, countryid)
	values (@cstreet, @ccity, @cstate, @czip, 'C', @personid, @ccountry), (@bstreet, @bcity, @bstate, @bzip, 'B', @personid, @bcountry)	

	fetch next from @contactcursor into @id, @lastname, @firstname, @mi, @salutation, @badge, @phone, @cell, @fax, @biography, @email, @password
									  , @ccity, @ccountry, @cstate, @cstreet, @czip
									  , @bcity, @bcountry, @bstate, @bstreet, @bzip

end

close @contactcursor
deallocate @contactcursor