/*
Reset mailing lists for Agribusiness to the correct state
*/

select * from mailinglists

/*
Populate the invitation list
*/
insert into mailinglistsxpeople ( mailinglistid, personid )
select 1, id from people

/*
Populate people who have applied
*/
insert into mailinglistsxpeople ( mailinglistid, personid )
select 8, people.id from applications
inner join people on people.userid = applications.userid

/*
Populate the registeed list
*/
insert into mailinglistsxpeople ( mailinglistid, personid )
select 2, personid from seminarpeople

/*
Populate people who still need to pay (4)
*/
insert into mailinglistsxpeople ( mailinglistid, personid )
select 4, personid from seminarpeople where paid = 0

/*
Populate people do not have a hotel (5)
*/
insert into mailinglistsxpeople ( mailinglistid, personid )
select 5, personid from seminarpeople where hotelconfirmation is null

/*
Populate people who do not have a photo (6)
*/
insert into mailinglistsxpeople ( mailinglistid, personid )
select 6, id from people where originalpicture is null

/*
Populate people who do not have a bio (7)
*/
insert into mailinglistsxpeople ( mailinglistid, personid )
select 7, id from people where biography is null

/*
Populate the attending list (paid through crp)
*/
insert into mailinglistsxpeople ( mailinglistid, personid )
select 3, personid from seminarpeople where paid = 1 order by personid