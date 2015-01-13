CREATE VIEW [dbo].[vEmailQueue]
	AS 

SELECT     eq.id, eq.Subject, eq.Body, ISNULL(eq.FromAddress, 'agribusiness@ucdavis.edu') AS fromaddress, CASE WHEN communicationoptionid = 'DR' AND 
                      dbo.isemail(m.email) = 1 THEN m.email WHEN communicationoptionid = 'DR' AND dbo.isemail(m.email) 
                      = 0 THEN 'agribusiness@ucdavis.edu' WHEN communicationoptionid = 'CA' AND dbo.isemail(m.email) = 1 AND dbo.isemail(c.email) 
                      = 1 THEN m.email + ',' + c.email WHEN communicationoptionid = 'CA' AND dbo.isemail(m.email) = 1 AND dbo.isemail(c.email) 
                      = 0 THEN m.email WHEN communicationoptionid = 'CA' AND dbo.isemail(m.email) = 0 AND dbo.isemail(c.email) 
                      = 1 THEN c.email WHEN communicationoptionid = 'AS' AND dbo.isemail(c.email) = 1 THEN c.email ELSE 'agribusiness@ucdavis.edu' END AS email
					  , p.id personid
FROM         dbo.EmailQueue AS eq INNER JOIN
                      dbo.People AS p ON eq.PersonId = p.id INNER JOIN
                      dbo.aspnet_Membership AS m ON p.UserId = m.UserId LEFT OUTER JOIN
                      dbo.Contacts AS c ON c.PersonId = p.id AND c.ContactTypeId = 'A'
WHERE     (eq.Pending = 1)