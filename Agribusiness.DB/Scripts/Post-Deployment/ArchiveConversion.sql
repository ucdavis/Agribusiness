--------------------------
-- Get the distinct years
--------------------------

insert into agribusiness.dbo.seminars (year, location, [begin], [end], releasetoattendees)
select distinct [YEAR], 'n/a' location
	, CAST(cast([year] as varchar(4)) + '-1-1' as datetime) [begin]
	, CAST(cast([year] as varchar(4)) + '-1-2' as datetime) [end]
	, 0 releaseToAttendees
from agribusinessarchive.dbo.seminars order by year
go


--------------------------
-- Get the list of contacts
--------------------------


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

go


--------------------------
-- Addresses
--------------------------


insert into agribusiness.dbo.addresses(personId, city, country, state, line1, zip, addresstypeid)
select personId, isnull(couriercity, 'n/a') city, couriercountry, isnull(courierstate, '') state, isnull(courierstreet, 'n/a') address, isnull(courierzip, 'n/a') zip, 'C'
from agribusinessarchive.dbo.contacts c
	inner join agribusinessarchive.dbo.vContacts vc on vc.contactId = c.id
where courierstreet is not null
  and (couriercountry = 'USA' or couriercountry is null)

go


insert into agribusiness.dbo.addresses(personid, line1, city, country, state, zip, addresstypeid)
select vc.personId, isnull(f.address, 'n/a') address, isnull(f.city, 'n/a') city, f.country, isnull(f.state, ''), isnull(f.zip, 'n/a') zip, 'B'
from agribusinessArchive.dbo.contactfirms
	inner join agribusinessArchive.dbo.firms f on f.f_id = contactfirms.firmid
	inner join agribusinessArchive.dbo.contacts c on c.c_id = contactfirms.contactid
	inner join agribusinessArchive.dbo.vContacts vc on vc.contactId = c.id
where country = 'USA' or country is null

go


--------------------------
-- Get the contacts
--------------------------


insert into agribusiness.dbo.contacts (firstname, lastname, email, phone, ext, contacttypeid, personid)
select assistantname, 'n/a', substring(isnull(assistantemail, 'n/a'), 1, 50)
	 , substring(isnull(assistantphone, 'n/a'), 1, 15)
	 , substring(assistantExt, 1, 10), 'A', vc.personId
from agribusinessarchive.dbo.contacts 
	inner join agribusinessarchive.dbo.vcontacts vc on contacts.id = vc.contactId
where assistantname is not null

go

insert into agribusiness.dbo.contacts (firstname, lastname, phone, ext, contacttypeid, personid)
select emergencyname, 'n/a'
	, substring(isnull(emergencyphone, 'n/a'), 1, 15)
	, substring(isnull(emergencyext, 'n/a'), 1, 10), 'E', vc.personId
from agribusinessarchive.dbo.contacts 
	inner join agribusinessarchive.dbo.vcontacts vc on contacts.id = vc.contactId
where emergencyname is not null

go


--------------------------
-- Get Firms
--------------------------

alter table agribusiness.dbo.firms
add archive_id uniqueidentifier null
go

create view vFirms as
select f.id firmId, af.id archiveId
from agribusiness.dbo.firms f
	inner join agribusinessarchive.dbo.firms af on f.archive_id = af.id
go

-- fill in null names with web address if it exists
update firms
set name = webaddress
where name is null and webaddress is not null

-- transport all the data over
insert into agribusiness.dbo.firms (firmcode, name, description, review, webaddress, archive_id)
select groupid, isnull(name, 'unknown') name, isnull(description, 'n/a') description, 0,webaddress, id
from agribusinessarchive.dbo.firms
order by f_id

/*
-- validation query, f_id and id should be in increasing order
select af.f_id, v.*
from agribusiness.dbo.firms v
	inner join agribusinessarchive.dbo.firms af on v.archive_id = af.id
order by firmcode, id
*/

--------------------------
-- Get the seminar people
--------------------------

alter table agribusiness.dbo.seminarpeople
add archive_id uniqueidentifier null
go

create view vSeminars as
select sp.id seminarPeopleId, s.id archiveId
from agribusinessarchive.dbo.seminars s
	inner join agribusiness.dbo.seminarpeople sp on s.id = sp.archive_id
go

insert into agribusiness.dbo.seminarpeople (seminarid, personId, title, firmid, paid, invite, contactinformationrelease, comments, archive_id)
select distinct ags.id seminarId, vc.personId, 'n/a' title
	, vf.firmId, s.isparticipant paid
	, isnull(isinvitee, 0) invitee
	, 0 contactInformationRelease
	, case
		when archivecomments is null then notes
		when notes is null and archivecomments is not null then '/* archive comments */' + archivecomments
		else isnull(notes, '') + '/* archive comments after this --> */ ' + isnull(archivecomments, '')
		end comments
	, s.id
from seminars s
	inner join contacts c on s.contactId = c.c_id
	inner join contactfirms cf on s.contactId = cf.contactid
	inner join firms f on cf.firmid = f.f_id
	inner join vfirms vf on f.id = vf.archiveId
	inner join vcontacts vc on c.id = vc.contactId
	inner join agribusiness.dbo.seminars ags on ags.[year] = s.[year]
where (s.iscaseexecutive = 1 or s.isdiscussiongrouplead = 1 or s.isfaculty = 1
		or s.issteeringcommittee = 1 or s.ispanelist = 1 or s.isparticipant = 1
		or s.isspeaker = 1 or s.isstaff = 1 or s.isvendor = 1)

go

--------------------------
-- Copy the seminar roles
--------------------------
insert into seminarpeoplexseminarroles (seminarpersonid, seminarroleid)
select sp.id, 'CE' from seminars s
	inner join agribusiness.dbo.seminarpeople sp on sp.archive_id = s.id
where s.iscaseexecutive = 1

insert into seminarpeoplexseminarroles (seminarpersonid, seminarroleid)
select sp.id, 'DL' from seminars s
	inner join agribusiness.dbo.seminarpeople sp on sp.archive_id = s.id
where s.isdiscussiongrouplead = 1

insert into seminarpeoplexseminarroles (seminarpersonid, seminarroleid)
select sp.id, 'FD' from seminars s
	inner join agribusiness.dbo.seminarpeople sp on sp.archive_id = s.id
where s.isfaculty = 1

insert into seminarpeoplexseminarroles (seminarpersonid, seminarroleid)
select sp.id, 'PN' from seminars s
	inner join agribusiness.dbo.seminarpeople sp on sp.archive_id = s.id
where s.ispanelist = 1

insert into seminarpeoplexseminarroles (seminarpersonid, seminarroleid)
select sp.id, 'SP' from seminars s
	inner join agribusiness.dbo.seminarpeople sp on sp.archive_id = s.id
where s.isspeaker = 1

insert into seminarpeoplexseminarroles (seminarpersonid, seminarroleid)
select sp.id, 'ST' from seminars s
	inner join agribusiness.dbo.seminarpeople sp on sp.archive_id = s.id
where s.isstaff = 1

insert into seminarpeoplexseminarroles (seminarpersonid, seminarroleid)
select sp.id, 'VD' from seminars s
	inner join agribusiness.dbo.seminarpeople sp on sp.archive_id = s.id
where s.isvendor = 1