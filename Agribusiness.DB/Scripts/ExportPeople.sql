/*
	Generates a list of people in a specific site
*/

declare @site varchar(15) = 'executive'

select people.id, seminarpeople.id spid, FirstName, LastName
	, aspnet_Membership.LoweredEmail
	, firms.name, SeminarPeople.title
	, Phone, PhoneExt
from people 
	inner join aspnet_Membership on aspnet_Membership.userid = people.userid
	inner join SeminarPeople on SeminarPeople.personid = people.id
	left outer join firms on SeminarPeople.firmid = firms.id
where people.id in (select personid from peoplexsites where siteid = @site)
  and seminarpeople.id = ( select max(sp.id) from seminarpeople sp where sp.personid = people.id )