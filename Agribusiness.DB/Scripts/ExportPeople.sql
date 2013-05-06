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



/*
	Generates list of all people in the database
*/
select p.id personid, p.firstname, p.mi, p.lastname
	, sp.Title, s.[Year], s.SiteId
	, am.loweredemail
	, bad.line1, bad.line2, bad.city, bad.[state], bad.zip, bad.addresstypeid, bad.countryid
	, cad.line1, cad.line2, cad.city, cad.[state], cad.zip, cad.addresstypeid, cad.countryid
from people p
	left outer join SeminarPeople sp on p.id = sp.PersonId
	left outer join seminars s on s.id = sp.seminarid
	inner join aspnet_Membership am on am.userid = p.userid
	left outer join addresses bad on p.id = bad.personid and bad.addresstypeid = 'B'
	left outer join addresses cad on p.id = cad.personid and cad.AddressTypeId = 'C'
order by p.lastname