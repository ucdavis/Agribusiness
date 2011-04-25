/*
setup tables for generation
*/
declare @firstnames table (name varchar(50))
declare @lastnames table (name varchar(50))

insert into @firstnames values ('James')
,('John')
,('Robert')
,('Michael')
,('Mary')
,('William')
,('David')
,('Richard')
,('Charles')
,('Joseph')
,('Thomas')
,('Patricia')
,('Christopher')
,('Linda')
,('Barbara')
,('Daniel')
,('Paul')
,('Mark')
,('Elizabeth')
,('Jennifer')
,('Maria')
,('Susan')
,('Margaret')
,('Dorothy')
,('Lisa')
,('Nancy')
,('Karen')

insert into @lastnames values ('Smith')
,('Johnson')
,('Williams')
,('Jones')
,('Brown')
,('Davis')
,('Miller')
,('Wilson')
,('Moore')
,('Taylor')
,('Anderson')
,('Thomas')
,('Jackson')
,('White')
,('Harris')
,('Martin')
,('Thompson')
,('Garcia')
,('Martinez')
,('Robinson')

declare @firmnames table (name varchar(50))

insert into @firmnames (name) values ('Acme, inc.')
,('Widget Corp')
,('123 Warehousing')
,('Demo Company')
,('Smith and Co.')
,('Foo Bars')
,('ABC Telecom')
,('Fake Brothers')
,('QWERTY Logistics')
,('Demo, inc.')
,('Sample Company')
,('Sample, inc')
,('Acme Corp')
,('Allied Biscuit')
,('Ankh-Sto Associates')
,('Extensive Enterprise')
,('Galaxy Corp')
,('Globo-Chem')
,('Mr. Sparkle')
,('Globex Corporation')
,('LexCorp')
,('LuthorCorp')
,('North Central Positronics')
,('Omni Consimer Products')
,('Praxis Corporation')
,('Sombra Corporation')
,('Sto Plains Holdings')
,('Tessier-Ashpool')
,('Wayne Enterprises')
,('Wentworth Industries')
,('ZiffCorp')
,('Bluth Company')

declare @cities table (name varchar(50))

insert into @cities (name) values ('California City')
,('Calimesa')
,('Canyon Lake')
,('Berkeley')
,('San Francisco')
,('Davis')
,('Chico')
,('Cloverdale')
,('Clovis')
,('Colfax')
,('Colton')

declare @jobtitles table (name varchar(50))

insert into @jobtitles (name) values ('CEO')
,('CFO')
,('CIO')
,('Head Produce Guy')
,('Head Cattleman')
,('VP Produce')
,('VP Strawberries')
,('President Operations')
,('VP Produce Safety')
,('VP Cow Patties')
,('VP Chicken Farms')
,('President of Corn')


/*
loop through and create the users
*/
declare @counter int = 0, @userid uniqueidentifier, @return_value int
declare @date datetime = getdate()

while @counter < 50
begin

	/*
	Stuff for name generation
	*/
	declare @firstname varchar(50), @lastname varchar(50)
	declare @dob datetime, @email varchar(100)

	set @firstname = (select top 1 * from @firstnames order by newid())
	set @lastname = (select top 1 * from @lastnames order by newid())

	set @dob = (	select	DOB
					from
					(
						select dateadd(month, -1 * abs(convert(varbinary, newid()) % (90 * 12)), getdate()) as DOB
					) d
					)
					
	set @email = @firstname + '.' + @lastname + '@fake.net'				
					
	/*
	phone number generator
	*/
	declare @num varchar(10)
	declare @phone varchar(15)

	set @num = (select RIGHT('1234567890'+cast(cast(9999999999*rand(checksum(newid())) as bigint) as varchar(10)),10))
	set @phone = substring(@num,1,3) + '-'
	set @phone += substring(@num, 4,3) + '-'
	set @phone += substring(@num, 7, 4)

	/*
	address generator
	*/
	declare @firmname varchar(200)
	declare @streetnum int, @line1 varchar(100), @city varchar(50), @state char(2), @zip varchar(10)
	declare @jobtitle varchar(50), @responsibilities varchar(max)

	set @firmname = (select top 1 * from @firmnames order by newid())
	set @streetnum = (convert(int, (floor(rand() * 99999))) % 10000)
	set @line1 = convert(varchar(4), @streetnum) + ' ' + @firmname + ' way'
	set @city = ( select top 1 * from @cities order by newid())
	set @state = ( select top 1 id from states order by newid())
	set @zip = convert(varchar(5), (convert (int, (floor(rand() * 9999999))) % 100000))

	set @jobtitle = (select top 1 name from @jobtitles order by newid())
	set @responsibilities = 'i am ' + @jobtitle + ' for ' + @firmname + ' in ' + @city

	set @userid = null

	EXEC	@return_value = [dbo].[aspnet_Membership_CreateUser]
			@ApplicationName = N'Agribusiness',
			@UserName = @email,
			@Password = N'password',
			@PasswordSalt = N'salt',
			@Email = @email,
			@PasswordQuestion = N'Q',
			@PasswordAnswer = N'A',
			@IsApproved = 1,
			@CurrentTimeUtc = @date,
			@CreateDate = @date,
			@UserId = @UserId OUTPUT
			
	insert into applications(firstname, lastname, firmname, firmaddressline1, firmcity
							, firmstate, firmzip, responsibilities, jobtitle, userid, seminarid)
	values (@firstname, @lastname, @firmname, @line1, @city, @state, @zip, @responsibilities, @jobtitle, @userid, 51)
	
	set @counter = @counter + 1

end


declare @id int, @cursor cursor, @cid1 int, @cid2 int
set @cursor = cursor for (select id from applications)

open @cursor

fetch next from @cursor into @id

while(@@fetch_status = 0)
begin

	set @cid1 = (select top 1 id from commodities order by newid())
	set @cid2 = (select top 1 id from commodities where id <> @cid1 order by newid())
	
	insert into ApplicationXCommodity (applicationid, commodityid) values (@id, @cid1), (@id, @cid2)

	fetch next from @cursor into @id

end

close @cursor
deallocate @cursor
