CREATE VIEW [dbo].[vEmailQueue]
	AS 

	SELECT     eq.id, eq.Subject, eq.Body, ISNULL(eq.FromAddress, 'agribusiness@ucdavis.edu') AS fromaddress, 
                      CASE WHEN communicationoptionid = 'DR' THEN u.loweredusername WHEN communicationoptionid = 'CA' THEN u.loweredusername + ',' + c.email WHEN communicationoptionid
                       = 'AS' THEN c.email END AS email
FROM         dbo.EmailQueue AS eq INNER JOIN
                      dbo.People AS p ON eq.PersonId = p.id INNER JOIN
                      dbo.aspnet_Users AS u ON p.UserId = u.UserId LEFT OUTER JOIN
                      dbo.Contacts AS c ON c.PersonId = p.id AND c.ContactTypeId = 'A'
WHERE     (eq.Pending = 1)