/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

/*
	Values for static lookup tables
*/

if not exists (select * from AddressTypes where id = 'B')
begin
insert AddressTypes (id, Name) values ('B', 'Business')
end
if not exists (select * from AddressTypes where id = 'C')
begin
insert AddressTypes (id, Name) values ('C', 'Courier')
end

if not exists (select * from ContactTypes where id = 'A')
begin
insert ContactTypes (id, Name) values ('A', 'Assistant')
end

if not exists (select * from ContactTypes where id = 'E')
begin
insert ContactTypes (id, Name) values ('E', 'Emergency')
end