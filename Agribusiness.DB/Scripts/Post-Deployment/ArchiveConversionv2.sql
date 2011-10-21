-- Check for Michale Mendes for duplicates

-----------------------------------------
-- Convert the contacts
-----------------------------------------

-- null out emails that don't exist
update agbizinvitees
set [email address] = null
where len([email address]) < 5


-- delcare the cursor
declare @cursor cursor

-- declare the variables
declare @firstname nvarchar(255), @lastname nvarchar(255), @mi nvarchar(max), @salutation nvarchar(max)
	  , @badge nvarchar(max), @phone nvarchar(max), @ext nvarchar(max), @cellphone nvarchar(max), @fax nvarchar(max)
	  , @biography nvarchar(max), @notes nvarchar(max), @email nvarchar(max)
	  , @assistantemailpreferred bit, @assistantemailpreferredcc bit, @title nvarchar(255), @firm nvarchar(255)
	  , @communicationoptions char(2)

declare @userid uniqueidentifier, @return_value int, @salt nvarchar(max), @date datetime

declare @seminarid int, @personid int
select @seminarid = max(id) from agribusiness.dbo.seminars where year = 2012

set @cursor = cursor for
	select ai.[first name], ai.[last name], c.mi, c.salutation
		, c.badge, isnull(c.phone, 'n/a'), c.ext, cellphone, fax
		, c.biography, c.notes
		, isnull(ai.[email address], ltrim(rtrim(ai.[first name])) + '.' + ltrim(rtrim(ai.[last name])) + '@') email
		--, c.assistantname, c.assistantphone, c.assistantext, c.assistantemail, c.assistantemailpreferred, c.assistantemailpreferredcc
		, assistantemailpreferred, assistantemailpreferredcc
		, ai.[title], ai.firm
	from AgbizInviteesFinal ai
		left outer join contacts c on (ai.[first name] = c.firstname and ai.[last name] = c.lastname) or ((ai.[email address] = c.email))
	where c.currentyear in (select max(icontacts.currentyear) from contacts icontacts where c.firstname = icontacts.firstname and c.lastname = icontacts.lastname)
		and c.currentseminaryear = 2012
	order by [last name]

open @cursor

fetch next from @cursor into @firstname, @lastname, @mi, @salutation, @badge, @phone, @ext, @cellphone, @fax, @biography, @notes, @email, @assistantemailpreferred, @assistantemailpreferredcc, @title, @firm

while (@@FETCH_STATUS = 0)
begin

	-- create the user account
	set @userid = null
	
	set @salt = convert(nvarchar(255), newid())
	set @date = getdate()
	
	EXEC	@return_value = [Agribusiness].[dbo].[aspnet_Membership_CreateUser]
		@ApplicationName = N'Agribusiness',
		@UserName = @email,
		@Password = 'password',
		@PasswordSalt = @salt,
		@Email = @email,
		@PasswordQuestion = N'Q',
		@PasswordAnswer = N'A',
		@IsApproved = 1,
		@CurrentTimeUtc = @date,
		@CreateDate = @date,
		@UserId = @UserId OUTPUT
	
	if (@userid is not null)
	begin
	
		-- set the communications options
		set @communicationoptions = 'DR'
		if (@assistantemailpreferred = 1) set @communicationoptions = 'AS'
		else if (@assistantemailpreferredcc = 1) set @communicationoptions = 'CA'

		insert into agribusiness.dbo.people (lastname, firstname, mi, salutation, badgename, phone, phoneext, cellphone, fax, biography, communicationoptionid, userid)
		values(@lastname, @firstname, @mi, @salutation, @badge, @phone, @ext, @cellphone, @fax, @biography, @communicationoptions, @userid)
	
		set @personid = (select max(id) from agribusiness.dbo.people where lastname = @lastname and firstname = @firstname)
		
		insert into agribusiness.dbo.invitations (personid, title, firmname, seminarid) values (@personid, @title, @firm, @seminarid)
	
	end
	
	fetch next from @cursor into @firstname, @lastname, @mi, @salutation, @badge, @phone, @ext, @cellphone, @fax, @biography, @notes, @email, @assistantemailpreferred, @assistantemailpreferredcc, @title, @firm

end

close @cursor
deallocate @cursor

set @cursor = cursor for
	select ai.[first name], ai.[last name], isnull(ai.[email address], ltrim(rtrim(ai.[first name])) + '.' + ltrim(rtrim(ai.[last name])) + '@') email
		, ai.title, ai.firm, isnull(ai.phone, 'n/a')
	from agbizinviteesfinal ai
	where ai.[first name]+ai.[last name] not in ( select firstname+lastname from agribusiness.dbo.people )

open @cursor

fetch next from @cursor into @firstname, @lastname, @email, @title, @firm, @phone

while(@@FETCH_STATUS = 0)
begin

		-- create the user account
	set @userid = null
	
	set @salt = convert(nvarchar(255), newid())
	set @date = getdate()
	
	EXEC	@return_value = [Agribusiness].[dbo].[aspnet_Membership_CreateUser]
		@ApplicationName = N'Agribusiness',
		@UserName = @email,
		@Password = 'password',
		@PasswordSalt = @salt,
		@Email = @email,
		@PasswordQuestion = N'Q',
		@PasswordAnswer = N'A',
		@IsApproved = 1,
		@CurrentTimeUtc = @date,
		@CreateDate = @date,
		@UserId = @UserId OUTPUT

	if (@userid is not null)
	begin

		insert into agribusiness.dbo.people(lastname, firstname, mi, communicationoptionid, userid, phone)
		values (@lastname, @firstname, @mi, 'DR', @userid, @phone)

		set @personid = (select max(id) from agribusiness.dbo.people where lastname = @lastname and firstname = @firstname)
		
		insert into agribusiness.dbo.invitations (personid, title, firmname, seminarid) values (@personid, @title, @firm, @seminarid)

	end

	fetch next from @cursor into @firstname, @lastname, @email, @title, @firm, @phone

end

close @cursor
deallocate @cursor

-----------------------------------------
-- Convert Addresses
-----------------------------------------

-- convert the codes that are not usa
update agbizinvitees set [firm's country] = 'CAN' where [firm's country] = 'Canada'
update agbizinvitees set [firm's country] = 'MEX' where [firm's country] = 'Mexico'
update agbizinvitees set [firm's country] = 'CHL' where [firm's country] = 'Chile'

update agbizinvitees set [courier's country] = 'MEX' where [courier's country] = 'Mexico'
update agbizinvitees set [courier's country] = 'CHL' where [courier's country] = 'Chile'

insert into agribusiness.dbo.addresses (personid, line1, city, state, countryid, zip, addresstypeid)
select distinct p.id, ai.[firm's address], isnulL(ai.[firm's city], 'n/a'), isnull(ai.[ state], 'n/a'), ai.[firm's country], isnull(ai.[zip], 'n/a'), 'B'
from agbizinviteesfinal ai
	inner join agribusiness.dbo.people p on ai.[first name] = p.firstname and ai.[last name] = p.lastname
where [firm's address] is not null

insert into agribusiness.dbo.addresses (personid, line1, city, state, countryid, zip, addresstypeid)
select distinct p.id, ai.[courier address], isnull(ai.[Courier Ciy], 'n/a'), isnull(ai.[courier state], 'n/a'), ai.[courier's country], isnull(ai.[courier zip code], 'n/a'), 'C'
from agbizinviteesfinal ai
	inner join agribusiness.dbo.people p on ai.[first name] = p.firstname and ai.[last name] = p.lastname
where [courier address] is not null

-----------------------------------------
-- Contacts
-----------------------------------------
insert into agribusiness.dbo.contacts (firstname, lastname, phone, ext, email, contacttypeid, personid)
select distinct ltrim(rtrim(reverse(substring(reverse(assistantname), charindex(' ', reverse(assistantname)) + 1, len(assistantname))))) firstname
	, ltrim(rtrim(reverse(substring(reverse(assistantname),1,charindex(' ',reverse(assistantname)))))) lastname
	, isnull(assistantphone, 'n/a'), assistantext, assistantemail, 'A', p.id
from agbizinviteesfinal ai
	inner join contacts c on (ai.[first name] = c.firstname and ai.[last name] = c.lastname) or (ai.[email address] = c.email)
	inner join agribusiness.dbo.people p on ai.[first name] = p.firstname and ai.[last name] = p.lastname
where assistantname is not null

-----------------------------------------
-- Steering Committee
-----------------------------------------

select * from agribusiness.dbo.people where lastname+firstname in ('McCorkleChester','Herl-VergaraCarlos ','SumnerDaniel','RuferChris','GillDavid','McCorkleKenneth','BarkleyRobert','BurrellMark','BoehmWilliam','JessenJon','HarrisJohn','YraceburuRob','CortopassiTom','CaplanKaren','CookRoberta','PerryWilliam','Van AlfenNeal','ParnagianDennis','ChandlerCarol','DuckhornDaniel','PhillimoreWilliam','ResnickStewart','BoswellJames','MusolinoSheila')

-----------------------------------------
-- Firms
-----------------------------------------

insert into agribusiness.dbo.firms (name, description, webaddress, review)
select distinct name, isnull(description, ''), webaddress, 1
from firms
where name is not null

-----------------------------------------
-- Validation Checks
-----------------------------------------

select distinct *
from (
select p.firstname, p.lastname, a.*
from agribusiness.dbo.addresses a
	inner join agribusiness.dbo.people p on a.personid = p.id
where a.personid in ( select personid from agribusiness.dbo.addresses where addresstypeid = 'B' group by personid having count(id) > 1 )
union
select p.firstname, p.lastname, a.*
from agribusiness.dbo.addresses a
	inner join agribusiness.dbo.people p on a.personid = p.id
where a.personid in ( select personid from agribusiness.dbo.addresses where addresstypeid = 'C' group by personid having count(id) > 1 )
) addresses

select p.firstname, p.lastname, c.*
from agribusiness.dbo.contacts c
	inner join agribusiness.dbo.people p on c.personid = p.id
where c.personid in ( select personid from agribusiness.dbo.contacts where contacttypeid = 'A' group by personid having count(id) > 1 )