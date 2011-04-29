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
insert AddressTypes (id, Name, required) values ('B', 'Business', 1)
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


if not exists (select * from commodities where Name = 'Agricultural Chemicals') begin insert into commodities (name) values ('Agricultural Chemicals') end
if not exists (select * from commodities where Name = 'Alfalfa') begin insert into commodities (name) values ('Alfalfa') end
if not exists (select * from commodities where Name = 'Almonds') begin insert into commodities (name) values ('Almonds') end
if not exists (select * from commodities where Name = 'Artichokes') begin insert into commodities (name) values ('Artichokes') end
if not exists (select * from commodities where Name = 'Asparagus') begin insert into commodities (name) values ('Asparagus') end
if not exists (select * from commodities where Name = 'Avocados') begin insert into commodities (name) values ('Avocados') end
if not exists (select * from commodities where Name = 'Beef') begin insert into commodities (name) values ('Beef') end
if not exists (select * from commodities where Name = 'Berries') begin insert into commodities (name) values ('Berries') end
if not exists (select * from commodities where Name = 'Beveages') begin insert into commodities (name) values ('Beveages') end
if not exists (select * from commodities where Name = 'Biofuels') begin insert into commodities (name) values ('Biofuels') end
if not exists (select * from commodities where Name = 'Potatoes') begin insert into commodities (name) values ('Potatoes') end
if not exists (select * from commodities where Name = 'Blueberries') begin insert into commodities (name) values ('Blueberries') end
if not exists (select * from commodities where Name = 'Bottled Water') begin insert into commodities (name) values ('Bottled Water') end
if not exists (select * from commodities where Name = 'Broccoli, Fresh') begin insert into commodities (name) values ('Broccoli, Fresh') end
if not exists (select * from commodities where Name = 'Carrots') begin insert into commodities (name) values ('Carrots') end
if not exists (select * from commodities where Name = 'Cherries') begin insert into commodities (name) values ('Cherries') end
if not exists (select * from commodities where Name = 'Citrus') begin insert into commodities (name) values ('Citrus') end
if not exists (select * from commodities where Name = 'Consumer Packaged Foods') begin insert into commodities (name) values ('Consumer Packaged Foods') end
if not exists (select * from commodities where Name = 'Cotton') begin insert into commodities (name) values ('Cotton') end
if not exists (select * from commodities where Name = 'Dairy') begin insert into commodities (name) values ('Dairy') end
if not exists (select * from commodities where Name = 'Dates') begin insert into commodities (name) values ('Dates') end
if not exists (select * from commodities where Name = 'Dressings') begin insert into commodities (name) values ('Dressings') end
if not exists (select * from commodities where Name = 'Eggs') begin insert into commodities (name) values ('Eggs') end
if not exists (select * from commodities where Name = 'Field Crops') begin insert into commodities (name) values ('Field Crops') end
if not exists (select * from commodities where Name = 'Floral') begin insert into commodities (name) values ('Floral') end
if not exists (select * from commodities where Name = 'Food, Processed') begin insert into commodities (name) values ('Food, Processed') end
if not exists (select * from commodities where Name = 'Fruit, Fresh') begin insert into commodities (name) values ('Fruit, Fresh') end
if not exists (select * from commodities where Name = 'Fruits, Processed') begin insert into commodities (name) values ('Fruits, Processed') end
if not exists (select * from commodities where Name = 'Fruits and Vegetables, Frozen') begin insert into commodities (name) values ('Fruits and Vegetables, Frozen') end
if not exists (select * from commodities where Name = 'Garlic') begin insert into commodities (name) values ('Garlic') end
if not exists (select * from commodities where Name = 'Grains') begin insert into commodities (name) values ('Grains') end
if not exists (select * from commodities where Name = 'Grapes, Raisins') begin insert into commodities (name) values ('Grapes, Raisins') end
if not exists (select * from commodities where Name = 'Grapes, Table') begin insert into commodities (name) values ('Grapes, Table') end
if not exists (select * from commodities where Name = 'Grapes, Wine') begin insert into commodities (name) values ('Grapes, Wine') end
if not exists (select * from commodities where Name = 'Ice Cream') begin insert into commodities (name) values ('Ice Cream') end
if not exists (select * from commodities where Name = 'Juice') begin insert into commodities (name) values ('Juice') end
if not exists (select * from commodities where Name = 'Leafy Greens, Fresh') begin insert into commodities (name) values ('Leafy Greens, Fresh') end
if not exists (select * from commodities where Name = 'Lemons') begin insert into commodities (name) values ('Lemons') end
if not exists (select * from commodities where Name = 'Lettuce') begin insert into commodities (name) values ('Lettuce') end
if not exists (select * from commodities where Name = 'Nectarines') begin insert into commodities (name) values ('Nectarines') end
if not exists (select * from commodities where Name = 'Nutraceuticals / Functional Foods') begin insert into commodities (name) values ('Nutraceuticals / Functional Foods') end
if not exists (select * from commodities where Name = 'Nuts') begin insert into commodities (name) values ('Nuts') end
if not exists (select * from commodities where Name = 'Oats') begin insert into commodities (name) values ('Oats') end
if not exists (select * from commodities where Name = 'Olives') begin insert into commodities (name) values ('Olives') end
if not exists (select * from commodities where Name = 'Packaged Salads') begin insert into commodities (name) values ('Packaged Salads') end
if not exists (select * from commodities where Name = 'Peaches') begin insert into commodities (name) values ('Peaches') end
if not exists (select * from commodities where Name = 'Pears') begin insert into commodities (name) values ('Pears') end
if not exists (select * from commodities where Name = 'Pistachios') begin insert into commodities (name) values ('Pistachios') end
if not exists (select * from commodities where Name = 'Plant Oils') begin insert into commodities (name) values ('Plant Oils') end
if not exists (select * from commodities where Name = 'Plums') begin insert into commodities (name) values ('Plums') end
if not exists (select * from commodities where Name = 'Pomegranates') begin insert into commodities (name) values ('Pomegranates') end
if not exists (select * from commodities where Name = 'Pork') begin insert into commodities (name) values ('Pork') end
if not exists (select * from commodities where Name = 'Produce, Fresh') begin insert into commodities (name) values ('Produce, Fresh') end
if not exists (select * from commodities where Name = 'Prunes') begin insert into commodities (name) values ('Prunes') end
if not exists (select * from commodities where Name = 'Raisins') begin insert into commodities (name) values ('Raisins') end
if not exists (select * from commodities where Name = 'Rice') begin insert into commodities (name) values ('Rice') end
if not exists (select * from commodities where Name = 'Seeds') begin insert into commodities (name) values ('Seeds') end
if not exists (select * from commodities where Name = 'Strawberries') begin insert into commodities (name) values ('Strawberries') end
if not exists (select * from commodities where Name = 'Sauces') begin insert into commodities (name) values ('Sauces') end
if not exists (select * from commodities where Name = 'Tomato Products') begin insert into commodities (name) values ('Tomato Products') end
if not exists (select * from commodities where Name = 'Tomatoes, Fresh') begin insert into commodities (name) values ('Tomatoes, Fresh') end
if not exists (select * from commodities where Name = 'Value-Added') begin insert into commodities (name) values ('Value-Added') end
if not exists (select * from commodities where Name = 'Vegetables, Fresh') begin insert into commodities (name) values ('Vegetables, Fresh') end
if not exists (select * from commodities where Name = 'Vegetables, Greenhouse') begin insert into commodities (name) values ('Vegetables, Greenhouse') end
if not exists (select * from commodities where Name = 'Walnuts') begin insert into commodities (name) values ('Walnuts') end
if not exists (select * from commodities where Name = 'Wheat') begin insert into commodities (name) values ('Wheat') end
if not exists (select * from commodities where Name = 'Wine') begin insert into commodities (name) values ('Wine') end
if not exists (select * from commodities where Name = 'Wood Products') begin insert into commodities (name) values ('Wood Products') end
if not exists (select * from commodities where Name = 'Wool') begin insert into commodities (name) values ('Wool') end
if not exists (select * from commodities where Name = 'Raspberries') begin insert into commodities (name) values ('Raspberries') end

if not exists (select * from states where id = 'AK') begin insert into states (id, name) values ('AK', 'ALASKA ') end
if not exists (select * from states where id = 'AL') begin insert into states (id, name) values ('AL', 'ALABAMA') end
if not exists (select * from states where id = 'AR') begin insert into states (id, name) values ('AR', 'ARKANSAS') end
if not exists (select * from states where id = 'AZ') begin insert into states (id, name) values ('AZ', 'ARIZONA') end
if not exists (select * from states where id = 'CA') begin insert into states (id, name) values ('CA', 'CALIFORNIA') end
if not exists (select * from states where id = 'CO') begin insert into states (id, name) values ('CO', 'COLORADO') end
if not exists (select * from states where id = 'CT') begin insert into states (id, name) values ('CT', 'CONNECTICUT') end
if not exists (select * from states where id = 'DC') begin insert into states (id, name) values ('DC', 'DISTRICT OF COLUMBIA') end
if not exists (select * from states where id = 'DE') begin insert into states (id, name) values ('DE', 'DELAWARE') end
if not exists (select * from states where id = 'FL') begin insert into states (id, name) values ('FL', 'FLORIDA') end
if not exists (select * from states where id = 'GA') begin insert into states (id, name) values ('GA', 'GEORGIA') end
if not exists (select * from states where id = 'HI') begin insert into states (id, name) values ('HI', 'HAWAII ') end
if not exists (select * from states where id = 'IA') begin insert into states (id, name) values ('IA', 'IOWA') end
if not exists (select * from states where id = 'ID') begin insert into states (id, name) values ('ID', 'IDAHO') end
if not exists (select * from states where id = 'IL') begin insert into states (id, name) values ('IL', 'ILLINOIS') end
if not exists (select * from states where id = 'IN') begin insert into states (id, name) values ('IN', 'INDIANA') end
if not exists (select * from states where id = 'KS') begin insert into states (id, name) values ('KS', 'KANSAS ') end
if not exists (select * from states where id = 'KY') begin insert into states (id, name) values ('KY', 'KENTUCKY') end
if not exists (select * from states where id = 'LA') begin insert into states (id, name) values ('LA', 'LOUISIANA') end
if not exists (select * from states where id = 'MA') begin insert into states (id, name) values ('MA', 'MASSACHUSETTS') end
if not exists (select * from states where id = 'MD') begin insert into states (id, name) values ('MD', 'MARYLAND') end
if not exists (select * from states where id = 'ME') begin insert into states (id, name) values ('ME', 'MAINE') end
if not exists (select * from states where id = 'MI') begin insert into states (id, name) values ('MI', 'MICHIGAN') end
if not exists (select * from states where id = 'MN') begin insert into states (id, name) values ('MN', 'MINNESOTA') end
if not exists (select * from states where id = 'MO') begin insert into states (id, name) values ('MO', 'MISSOURI') end
if not exists (select * from states where id = 'MS') begin insert into states (id, name) values ('MS', 'MISSISSIPPI') end
if not exists (select * from states where id = 'MT') begin insert into states (id, name) values ('MT', 'MONTANA') end
if not exists (select * from states where id = 'NC') begin insert into states (id, name) values ('NC', 'NORTH CAROLINA') end
if not exists (select * from states where id = 'ND') begin insert into states (id, name) values ('ND', 'NORTH DAKOTA') end
if not exists (select * from states where id = 'NE') begin insert into states (id, name) values ('NE', 'NEBRASKA') end
if not exists (select * from states where id = 'NH') begin insert into states (id, name) values ('NH', 'NEW HAMPSHIRE') end
if not exists (select * from states where id = 'NJ') begin insert into states (id, name) values ('NJ', 'NEW JERSEY') end
if not exists (select * from states where id = 'NM') begin insert into states (id, name) values ('NM', 'NEW MEXICO') end
if not exists (select * from states where id = 'NV') begin insert into states (id, name) values ('NV', 'NEVADA ') end
if not exists (select * from states where id = 'NY') begin insert into states (id, name) values ('NY', 'NEW YORK') end
if not exists (select * from states where id = 'OH') begin insert into states (id, name) values ('OH', 'OHIO') end
if not exists (select * from states where id = 'OK') begin insert into states (id, name) values ('OK', 'OKLAHOMA') end
if not exists (select * from states where id = 'OR') begin insert into states (id, name) values ('OR', 'OREGON ') end
if not exists (select * from states where id = 'PA') begin insert into states (id, name) values ('PA', 'PENNSYLVANIA') end
if not exists (select * from states where id = 'PR') begin insert into states (id, name) values ('PR', 'PUERTO RICO') end
if not exists (select * from states where id = 'RI') begin insert into states (id, name) values ('RI', 'RHODE ISLAND') end
if not exists (select * from states where id = 'SC') begin insert into states (id, name) values ('SC', 'SOUTH CAROLINA') end
if not exists (select * from states where id = 'SD') begin insert into states (id, name) values ('SD', 'SOUTH DAKOTA') end
if not exists (select * from states where id = 'TN') begin insert into states (id, name) values ('TN', 'TENNESSEE') end
if not exists (select * from states where id = 'TX') begin insert into states (id, name) values ('TX', 'TEXAS') end
if not exists (select * from states where id = 'UT') begin insert into states (id, name) values ('UT', 'UTAH') end
if not exists (select * from states where id = 'VA') begin insert into states (id, name) values ('VA', 'VIRGINIA') end
if not exists (select * from states where id = 'VI') begin insert into states (id, name) values ('VI', 'U.S. VIRGIN ISLANDS') end
if not exists (select * from states where id = 'VT') begin insert into states (id, name) values ('VT', 'VERMONT') end
if not exists (select * from states where id = 'WA') begin insert into states (id, name) values ('WA', 'WASHINGTON') end
if not exists (select * from states where id = 'WI') begin insert into states (id, name) values ('WI', 'WISCONSIN') end
if not exists (select * from states where id = 'WV') begin insert into states (id, name) values ('WV', 'WEST VIRGINIA') end
if not exists (select * from states where id = 'WY') begin insert into states (id, name) values ('WY', 'WYOMING') end

if not exists (select * from seminarroles where id = 'CA') begin insert into seminarroles (id, name, discount, description) values ('CA', 'Case Study Author', 10.00, 'Person writing a Case Study') end
if not exists (select * from seminarroles where id = 'CE') begin insert into seminarroles (id, name, discount, description) values ('CE', 'Case Executive', 10.00, 'Executive attached to a Case Study') end
if not exists (select * from seminarroles where id = 'DL') begin insert into seminarroles (id, name, discount, description) values ('DL', 'Discussion Group Leader', 10.00, 'Leader iin the Discussion Group Breakouts') end
if not exists (select * from seminarroles where id = 'FD') begin insert into seminarroles (id, name, discount, description) values ('FD', 'Faculty Director', 10.00, 'UCD appointed lead on the conference') end
if not exists (select * from seminarroles where id = 'PN') begin insert into seminarroles (id, name, discount, description) values ('PN', 'Panelist', 10.00, 'One of a few speakers during a session') end
if not exists (select * from seminarroles where id = 'PT') begin insert into seminarroles (id, name, discount, description) values ('PT', 'Participant', 10.00, 'Paying Attendee') end
if not exists (select * from seminarroles where id = 'SC') begin insert into seminarroles (id, name, discount, description) values ('SC', 'Steering Committee', 10.00, 'Appointed committee who makes decisions on themes for case studies and speakers') end
if not exists (select * from seminarroles where id = 'SP') begin insert into seminarroles (id, name, discount, description) values ('SP', 'Speaker', 10.00, 'Paid Session Speaker') end
if not exists (select * from seminarroles where id = 'ST') begin insert into seminarroles (id, name, discount, description) values ('ST', 'Staff', 10.00, 'Anyone Working on/at and Attending Conference') end
if not exists (select * from seminarroles where id = 'VD') begin insert into seminarroles (id, name, discount, description) values ('VD', 'Vendor', 10.00, 'Businesses we work with for this event') end

/*
Necessary for the membership provider to work
*/
insert into aspnet_SchemaVersions (Feature, CompatibleSchemaVersion, IsCurrentVersion) values ('common', 1, 1)
insert into aspnet_SchemaVersions (Feature, CompatibleSchemaVersion, IsCurrentVersion) values ('membership', 1, 1)
insert into aspnet_Applications (ApplicationName, LoweredApplicationName, ApplicationId) values ('Agribusiness', 'agribusiness', '17a4ebcf-1b03-4096-92a4-263ca67d7f5a')