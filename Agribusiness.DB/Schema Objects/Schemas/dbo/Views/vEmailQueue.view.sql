CREATE VIEW [dbo].[vEmailQueue]
	AS 

SELECT     eq.id, eq.Subject, eq.Body, ISNULL(eq.FromAddress, 'agribusiness@ucdavis.edu') AS fromaddress
			,CASE 
				WHEN communicationoptionid = 'DR' and dbo.isemail(u.loweredusername) = 1 THEN u.loweredusername 
				WHEN communicationoptionid = 'DR' and dbo.isemail(u.loweredusername) = 0 then 'agribusiness.@ucdavis.edu'
				WHEN communicationoptionid = 'CA' and dbo.isemail(u.loweredusername) = 1 and dbo.isemail(c.email) = 1 THEN u.loweredusername + ',' + c.email 
				WHEN communicationoptionid = 'CA' and dbo.isemail(u.loweredusername) = 1 and dbo.isemail(c.email) = 0 THEN u.loweredusername
				WHEN communicationoptionid = 'CA' and dbo.isemail(u.loweredusername) = 0 and dbo.isemail(c.email) = 1 THEN c.email
				WHEN communicationoptionid = 'AS' and dbo.isemail(c.email) = 1 THEN c.email 
				ELSE 'agribusiness@ucdavis.edu'
				END AS email
FROM         dbo.EmailQueue AS eq INNER JOIN
                      dbo.People AS p ON eq.PersonId = p.id INNER JOIN
                      dbo.aspnet_Users AS u ON p.UserId = u.UserId LEFT OUTER JOIN
                      dbo.Contacts AS c ON c.PersonId = p.id AND c.ContactTypeId = 'A'
WHERE     (eq.Pending = 1)