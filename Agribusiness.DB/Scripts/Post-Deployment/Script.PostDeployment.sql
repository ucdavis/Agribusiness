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

if not exists (select * from commodities where Name = 'Citrus') begin insert into commodities (name) values ('Citrus') end
if not exists (select * from commodities where Name = 'Other Tree Fruit') begin insert into commodities (name) values ('Other Tree Fruit') end
if not exists (select * from commodities where Name = 'Pistachios') begin insert into commodities (name) values ('Pistachios') end
if not exists (select * from commodities where Name = 'Almonds') begin insert into commodities (name) values ('Almonds') end
if not exists (select * from commodities where Name = 'Walnuts') begin insert into commodities (name) values ('Walnuts') end
if not exists (select * from commodities where Name = 'Dairy') begin insert into commodities (name) values ('Dairy') end
if not exists (select * from commodities where Name = 'Fresh Vegetables and Produce') begin insert into commodities (name) values ('Fresh Vegetables and Produce') end
if not exists (select * from commodities where Name = 'Processing Tomatoes') begin insert into commodities (name) values ('Processing Tomatoes') end
if not exists (select * from commodities where Name = 'Other Processing Vegetables') begin insert into commodities (name) values ('Other Processing Vegetables') end
if not exists (select * from commodities where Name = 'Raisin Grapes') begin insert into commodities (name) values ('Raisin Grapes') end
if not exists (select * from commodities where Name = 'Wine Grapes') begin insert into commodities (name) values ('Wine Grapes') end
if not exists (select * from commodities where Name = 'Table Grapes') begin insert into commodities (name) values ('Table Grapes') end
if not exists (select * from commodities where Name = 'Wine') begin insert into commodities (name) values ('Wine') end
if not exists (select * from commodities where Name = 'Eggs') begin insert into commodities (name) values ('Eggs') end
if not exists (select * from commodities where Name = 'Poultry') begin insert into commodities (name) values ('Poultry') end
if not exists (select * from commodities where Name = 'Cattle and Other Livestock') begin insert into commodities (name) values ('Cattle and Other Livestock') end
if not exists (select * from commodities where Name = 'Flowers and Foliage ') begin insert into commodities (name) values ('Flowers and Foliage ') end
if not exists (select * from commodities where Name = 'Other Nursery and Greenhouse') begin insert into commodities (name) values ('Other Nursery and Greenhouse') end
if not exists (select * from commodities where Name = 'Berries') begin insert into commodities (name) values ('Berries') end
if not exists (select * from commodities where Name = 'Corn') begin insert into commodities (name) values ('Corn') end
if not exists (select * from commodities where Name = 'Alfalfa') begin insert into commodities (name) values ('Alfalfa') end
if not exists (select * from commodities where Name = 'Wheat') begin insert into commodities (name) values ('Wheat') end
if not exists (select * from commodities where Name = 'Cotton') begin insert into commodities (name) values ('Cotton') end
if not exists (select * from commodities where Name = 'Other Field Crops') begin insert into commodities (name) values ('Other Field Crops') end
if not exists (select * from commodities where Name = 'Seeds') begin insert into commodities (name) values ('Seeds') end
if not exists (select * from commodities where Name = 'Biofuels') begin insert into commodities (name) values ('Biofuels') end


/*
	Removed as of 12/7/2011, By order of Dan
*/
--if not exists (select * from commodities where Name = 'Agricultural Chemicals') begin insert into commodities (name) values ('Agricultural Chemicals') end
--if not exists (select * from commodities where Name = 'Almonds') begin insert into commodities (name) values ('Almonds') end
--if not exists (select * from commodities where Name = 'Artichokes') begin insert into commodities (name) values ('Artichokes') end
--if not exists (select * from commodities where Name = 'Asparagus') begin insert into commodities (name) values ('Asparagus') end
--if not exists (select * from commodities where Name = 'Avocados') begin insert into commodities (name) values ('Avocados') end
--if not exists (select * from commodities where Name = 'Beef') begin insert into commodities (name) values ('Beef') end
--if not exists (select * from commodities where Name = 'Berries') begin insert into commodities (name) values ('Berries') end
--if not exists (select * from commodities where Name = 'Beveages') begin insert into commodities (name) values ('Beveages') end
--if not exists (select * from commodities where Name = 'Potatoes') begin insert into commodities (name) values ('Potatoes') end
--if not exists (select * from commodities where Name = 'Blueberries') begin insert into commodities (name) values ('Blueberries') end
--if not exists (select * from commodities where Name = 'Bottled Water') begin insert into commodities (name) values ('Bottled Water') end
--if not exists (select * from commodities where Name = 'Broccoli, Fresh') begin insert into commodities (name) values ('Broccoli, Fresh') end
--if not exists (select * from commodities where Name = 'Carrots') begin insert into commodities (name) values ('Carrots') end
--if not exists (select * from commodities where Name = 'Cherries') begin insert into commodities (name) values ('Cherries') end
--if not exists (select * from commodities where Name = 'Consumer Packaged Foods') begin insert into commodities (name) values ('Consumer Packaged Foods') end
--if not exists (select * from commodities where Name = 'Dates') begin insert into commodities (name) values ('Dates') end
--if not exists (select * from commodities where Name = 'Dressings') begin insert into commodities (name) values ('Dressings') end
--if not exists (select * from commodities where Name = 'Field Crops') begin insert into commodities (name) values ('Field Crops') end
--if not exists (select * from commodities where Name = 'Floral') begin insert into commodities (name) values ('Floral') end
--if not exists (select * from commodities where Name = 'Food, Processed') begin insert into commodities (name) values ('Food, Processed') end
--if not exists (select * from commodities where Name = 'Fruit, Fresh') begin insert into commodities (name) values ('Fruit, Fresh') end
--if not exists (select * from commodities where Name = 'Fruits, Processed') begin insert into commodities (name) values ('Fruits, Processed') end
--if not exists (select * from commodities where Name = 'Fruits and Vegetables, Frozen') begin insert into commodities (name) values ('Fruits and Vegetables, Frozen') end
--if not exists (select * from commodities where Name = 'Garlic') begin insert into commodities (name) values ('Garlic') end
--if not exists (select * from commodities where Name = 'Grains') begin insert into commodities (name) values ('Grains') end
--if not exists (select * from commodities where Name = 'Ice Cream') begin insert into commodities (name) values ('Ice Cream') end
--if not exists (select * from commodities where Name = 'Juice') begin insert into commodities (name) values ('Juice') end
--if not exists (select * from commodities where Name = 'Leafy Greens, Fresh') begin insert into commodities (name) values ('Leafy Greens, Fresh') end
--if not exists (select * from commodities where Name = 'Lemons') begin insert into commodities (name) values ('Lemons') end
--if not exists (select * from commodities where Name = 'Lettuce') begin insert into commodities (name) values ('Lettuce') end
--if not exists (select * from commodities where Name = 'Nectarines') begin insert into commodities (name) values ('Nectarines') end
--if not exists (select * from commodities where Name = 'Nutraceuticals / Functional Foods') begin insert into commodities (name) values ('Nutraceuticals / Functional Foods') end
--if not exists (select * from commodities where Name = 'Nuts') begin insert into commodities (name) values ('Nuts') end
--if not exists (select * from commodities where Name = 'Oats') begin insert into commodities (name) values ('Oats') end
--if not exists (select * from commodities where Name = 'Olives') begin insert into commodities (name) values ('Olives') end
--if not exists (select * from commodities where Name = 'Packaged Salads') begin insert into commodities (name) values ('Packaged Salads') end
--if not exists (select * from commodities where Name = 'Peaches') begin insert into commodities (name) values ('Peaches') end
--if not exists (select * from commodities where Name = 'Pears') begin insert into commodities (name) values ('Pears') end
--if not exists (select * from commodities where Name = 'Plant Oils') begin insert into commodities (name) values ('Plant Oils') end
--if not exists (select * from commodities where Name = 'Plums') begin insert into commodities (name) values ('Plums') end
--if not exists (select * from commodities where Name = 'Pomegranates') begin insert into commodities (name) values ('Pomegranates') end
--if not exists (select * from commodities where Name = 'Pork') begin insert into commodities (name) values ('Pork') end
--if not exists (select * from commodities where Name = 'Produce, Fresh') begin insert into commodities (name) values ('Produce, Fresh') end
--if not exists (select * from commodities where Name = 'Prunes') begin insert into commodities (name) values ('Prunes') end
--if not exists (select * from commodities where Name = 'Raisins') begin insert into commodities (name) values ('Raisins') end
--if not exists (select * from commodities where Name = 'Rice') begin insert into commodities (name) values ('Rice') end
--if not exists (select * from commodities where Name = 'Seeds') begin insert into commodities (name) values ('Seeds') end
--if not exists (select * from commodities where Name = 'Strawberries') begin insert into commodities (name) values ('Strawberries') end
--if not exists (select * from commodities where Name = 'Sauces') begin insert into commodities (name) values ('Sauces') end
--if not exists (select * from commodities where Name = 'Tomatoes, Fresh') begin insert into commodities (name) values ('Tomatoes, Fresh') end
--if not exists (select * from commodities where Name = 'Value-Added') begin insert into commodities (name) values ('Value-Added') end
--if not exists (select * from commodities where Name = 'Vegetables, Fresh') begin insert into commodities (name) values ('Vegetables, Fresh') end
--if not exists (select * from commodities where Name = 'Vegetables, Greenhouse') begin insert into commodities (name) values ('Vegetables, Greenhouse') end
--if not exists (select * from commodities where Name = 'Wood Products') begin insert into commodities (name) values ('Wood Products') end
--if not exists (select * from commodities where Name = 'Wool') begin insert into commodities (name) values ('Wool') end
--if not exists (select * from commodities where Name = 'Raspberries') begin insert into commodities (name) values ('Raspberries') end

/*
	removed as of 5/4/2011

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
*/

if not exists (select * from seminarroles where id = 'CA') begin insert into seminarroles (id, name, discount, description) values ('CA', 'Case Study Author', 10.00, 'Person writing a Case Study') end
if not exists (select * from seminarroles where id = 'CE') begin insert into seminarroles (id, name, discount, description) values ('CE', 'Case Executive', 10.00, 'Executive attached to a Case Study') end
if not exists (select * from seminarroles where id = 'DL') begin insert into seminarroles (id, name, discount, description) values ('DL', 'Discussion Group Leader', 10.00, 'Leader iin the Discussion Group Breakouts') end
if not exists (select * from seminarroles where id = 'FD') begin insert into seminarroles (id, name, discount, description) values ('FD', 'Faculty Director', 10.00, 'UCD appointed lead on the conference') end
if not exists (select * from seminarroles where id = 'PN') begin insert into seminarroles (id, name, discount, description) values ('PN', 'Panelist', 10.00, 'One of a few speakers during a session') end
if not exists (select * from seminarroles where id = 'PT') begin insert into seminarroles (id, name, discount, description) values ('PT', 'Participant', 10.00, 'Paying Attendee') end
if not exists (select * from seminarroles where id = 'FT') begin insert into seminarroles (id, name, discount, description) values ('FT', 'Faculty', 10.00, 'Faculty') end
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


if not exists (select * from countries where id = 'AND') begin insert into countries (id, name) values ('AND', 'Andorra') end
if not exists (select * from countries where id = 'ARE') begin insert into countries (id, name) values ('ARE', 'United Arab Emirates') end
if not exists (select * from countries where id = 'AFG') begin insert into countries (id, name) values ('AFG', 'Afghanistan') end
if not exists (select * from countries where id = 'ATG') begin insert into countries (id, name) values ('ATG', 'Antigua and Barbuda') end
if not exists (select * from countries where id = 'AIA') begin insert into countries (id, name) values ('AIA', 'Anguilla') end
if not exists (select * from countries where id = 'ALB') begin insert into countries (id, name) values ('ALB', 'Albania') end
if not exists (select * from countries where id = 'ARM') begin insert into countries (id, name) values ('ARM', 'Armenia') end
if not exists (select * from countries where id = 'ANT') begin insert into countries (id, name) values ('ANT', 'Netherlands Antilles') end
if not exists (select * from countries where id = 'AGO') begin insert into countries (id, name) values ('AGO', 'Angola') end
if not exists (select * from countries where id = 'ARG') begin insert into countries (id, name) values ('ARG', 'Argentina') end
if not exists (select * from countries where id = 'ASM') begin insert into countries (id, name) values ('ASM', 'American Samoa') end
if not exists (select * from countries where id = 'AUT') begin insert into countries (id, name) values ('AUT', 'Austria') end
if not exists (select * from countries where id = 'AUS') begin insert into countries (id, name) values ('AUS', 'Australia') end
if not exists (select * from countries where id = 'ABW') begin insert into countries (id, name) values ('ABW', 'Aruba') end
if not exists (select * from countries where id = 'AZE') begin insert into countries (id, name) values ('AZE', 'Azerbaijan') end
if not exists (select * from countries where id = 'BIH') begin insert into countries (id, name) values ('BIH', 'Bosnia and Herzegovina') end
if not exists (select * from countries where id = 'BRB') begin insert into countries (id, name) values ('BRB', 'Barbados') end
if not exists (select * from countries where id = 'BGD') begin insert into countries (id, name) values ('BGD', 'Bangladesh') end
if not exists (select * from countries where id = 'BEL') begin insert into countries (id, name) values ('BEL', 'Belgium') end
if not exists (select * from countries where id = 'BFA') begin insert into countries (id, name) values ('BFA', 'Burkina Faso') end
if not exists (select * from countries where id = 'BGR') begin insert into countries (id, name) values ('BGR', 'Bulgaria') end
if not exists (select * from countries where id = 'BHR') begin insert into countries (id, name) values ('BHR', 'Bahrain') end
if not exists (select * from countries where id = 'BDI') begin insert into countries (id, name) values ('BDI', 'Burundi') end
if not exists (select * from countries where id = 'BEN') begin insert into countries (id, name) values ('BEN', 'Benin') end
if not exists (select * from countries where id = 'BMU') begin insert into countries (id, name) values ('BMU', 'Bermuda') end
if not exists (select * from countries where id = 'BRN') begin insert into countries (id, name) values ('BRN', 'Brunei Darussalam') end
if not exists (select * from countries where id = 'BOL') begin insert into countries (id, name) values ('BOL', 'Bolivia') end
if not exists (select * from countries where id = 'BRA') begin insert into countries (id, name) values ('BRA', 'Brazil') end
if not exists (select * from countries where id = 'BHS') begin insert into countries (id, name) values ('BHS', 'Bahamas') end
if not exists (select * from countries where id = 'BTN') begin insert into countries (id, name) values ('BTN', 'Bhutan') end
if not exists (select * from countries where id = 'BWA') begin insert into countries (id, name) values ('BWA', 'Botswana') end
if not exists (select * from countries where id = 'BLR') begin insert into countries (id, name) values ('BLR', 'Belarus') end
if not exists (select * from countries where id = 'BLZ') begin insert into countries (id, name) values ('BLZ', 'Belize') end
if not exists (select * from countries where id = 'CAN') begin insert into countries (id, name) values ('CAN', 'Canada') end
if not exists (select * from countries where id = 'COD') begin insert into countries (id, name) values ('COD', 'Congo, the Democratic Republic of the') end
if not exists (select * from countries where id = 'CAF') begin insert into countries (id, name) values ('CAF', 'Central African Republic') end
if not exists (select * from countries where id = 'COG') begin insert into countries (id, name) values ('COG', 'Congo') end
if not exists (select * from countries where id = 'CHE') begin insert into countries (id, name) values ('CHE', 'Switzerland') end
if not exists (select * from countries where id = 'CIV') begin insert into countries (id, name) values ('CIV', 'Cote D''Ivoire') end
if not exists (select * from countries where id = 'COK') begin insert into countries (id, name) values ('COK', 'Cook Islands') end
if not exists (select * from countries where id = 'CHL') begin insert into countries (id, name) values ('CHL', 'Chile') end
if not exists (select * from countries where id = 'CMR') begin insert into countries (id, name) values ('CMR', 'Cameroon') end
if not exists (select * from countries where id = 'CHN') begin insert into countries (id, name) values ('CHN', 'China') end
if not exists (select * from countries where id = 'COL') begin insert into countries (id, name) values ('COL', 'Colombia') end
if not exists (select * from countries where id = 'CRI') begin insert into countries (id, name) values ('CRI', 'Costa Rica') end
if not exists (select * from countries where id = 'CUB') begin insert into countries (id, name) values ('CUB', 'Cuba') end
if not exists (select * from countries where id = 'CPV') begin insert into countries (id, name) values ('CPV', 'Cape Verde') end
if not exists (select * from countries where id = 'CYP') begin insert into countries (id, name) values ('CYP', 'Cyprus') end
if not exists (select * from countries where id = 'CZE') begin insert into countries (id, name) values ('CZE', 'Czech Republic') end
if not exists (select * from countries where id = 'DEU') begin insert into countries (id, name) values ('DEU', 'Germany') end
if not exists (select * from countries where id = 'DJI') begin insert into countries (id, name) values ('DJI', 'Djibouti') end
if not exists (select * from countries where id = 'DNK') begin insert into countries (id, name) values ('DNK', 'Denmark') end
if not exists (select * from countries where id = 'DMA') begin insert into countries (id, name) values ('DMA', 'Dominica') end
if not exists (select * from countries where id = 'DOM') begin insert into countries (id, name) values ('DOM', 'Dominican Republic') end
if not exists (select * from countries where id = 'DZA') begin insert into countries (id, name) values ('DZA', 'Algeria') end
if not exists (select * from countries where id = 'ECU') begin insert into countries (id, name) values ('ECU', 'Ecuador') end
if not exists (select * from countries where id = 'EST') begin insert into countries (id, name) values ('EST', 'Estonia') end
if not exists (select * from countries where id = 'EGY') begin insert into countries (id, name) values ('EGY', 'Egypt') end
if not exists (select * from countries where id = 'ESH') begin insert into countries (id, name) values ('ESH', 'Western Sahara') end
if not exists (select * from countries where id = 'ERI') begin insert into countries (id, name) values ('ERI', 'Eritrea') end
if not exists (select * from countries where id = 'ESP') begin insert into countries (id, name) values ('ESP', 'Spain') end
if not exists (select * from countries where id = 'ETH') begin insert into countries (id, name) values ('ETH', 'Ethiopia') end
if not exists (select * from countries where id = 'FIN') begin insert into countries (id, name) values ('FIN', 'Finland') end
if not exists (select * from countries where id = 'FJI') begin insert into countries (id, name) values ('FJI', 'Fiji') end
if not exists (select * from countries where id = 'FLK') begin insert into countries (id, name) values ('FLK', 'Falkland Islands (Malvinas)') end
if not exists (select * from countries where id = 'FSM') begin insert into countries (id, name) values ('FSM', 'Micronesia, Federated States of') end
if not exists (select * from countries where id = 'FRO') begin insert into countries (id, name) values ('FRO', 'Faroe Islands') end
if not exists (select * from countries where id = 'FRA') begin insert into countries (id, name) values ('FRA', 'France') end
if not exists (select * from countries where id = 'GAB') begin insert into countries (id, name) values ('GAB', 'Gabon') end
if not exists (select * from countries where id = 'GBR') begin insert into countries (id, name) values ('GBR', 'United Kingdom') end
if not exists (select * from countries where id = 'GRD') begin insert into countries (id, name) values ('GRD', 'Grenada') end
if not exists (select * from countries where id = 'GEO') begin insert into countries (id, name) values ('GEO', 'Georgia') end
if not exists (select * from countries where id = 'GUF') begin insert into countries (id, name) values ('GUF', 'French Guiana') end
if not exists (select * from countries where id = 'GHA') begin insert into countries (id, name) values ('GHA', 'Ghana') end
if not exists (select * from countries where id = 'GIB') begin insert into countries (id, name) values ('GIB', 'Gibraltar') end
if not exists (select * from countries where id = 'GRL') begin insert into countries (id, name) values ('GRL', 'Greenland') end
if not exists (select * from countries where id = 'GMB') begin insert into countries (id, name) values ('GMB', 'Gambia') end
if not exists (select * from countries where id = 'GIN') begin insert into countries (id, name) values ('GIN', 'Guinea') end
if not exists (select * from countries where id = 'GLP') begin insert into countries (id, name) values ('GLP', 'Guadeloupe') end
if not exists (select * from countries where id = 'GNQ') begin insert into countries (id, name) values ('GNQ', 'Equatorial Guinea') end
if not exists (select * from countries where id = 'GRC') begin insert into countries (id, name) values ('GRC', 'Greece') end
if not exists (select * from countries where id = 'GTM') begin insert into countries (id, name) values ('GTM', 'Guatemala') end
if not exists (select * from countries where id = 'GUM') begin insert into countries (id, name) values ('GUM', 'Guam') end
if not exists (select * from countries where id = 'GNB') begin insert into countries (id, name) values ('GNB', 'Guinea-Bissau') end
if not exists (select * from countries where id = 'GUY') begin insert into countries (id, name) values ('GUY', 'Guyana') end
if not exists (select * from countries where id = 'HKG') begin insert into countries (id, name) values ('HKG', 'Hong Kong') end
if not exists (select * from countries where id = 'HND') begin insert into countries (id, name) values ('HND', 'Honduras') end
if not exists (select * from countries where id = 'HRV') begin insert into countries (id, name) values ('HRV', 'Croatia') end
if not exists (select * from countries where id = 'HTI') begin insert into countries (id, name) values ('HTI', 'Haiti') end
if not exists (select * from countries where id = 'HUN') begin insert into countries (id, name) values ('HUN', 'Hungary') end
if not exists (select * from countries where id = 'IDN') begin insert into countries (id, name) values ('IDN', 'Indonesia') end
if not exists (select * from countries where id = 'IRL') begin insert into countries (id, name) values ('IRL', 'Ireland') end
if not exists (select * from countries where id = 'ISR') begin insert into countries (id, name) values ('ISR', 'Israel') end
if not exists (select * from countries where id = 'IND') begin insert into countries (id, name) values ('IND', 'India') end
if not exists (select * from countries where id = 'IRQ') begin insert into countries (id, name) values ('IRQ', 'Iraq') end
if not exists (select * from countries where id = 'IRN') begin insert into countries (id, name) values ('IRN', 'Iran, Islamic Republic of') end
if not exists (select * from countries where id = 'ISL') begin insert into countries (id, name) values ('ISL', 'Iceland') end
if not exists (select * from countries where id = 'ITA') begin insert into countries (id, name) values ('ITA', 'Italy') end
if not exists (select * from countries where id = 'JAM') begin insert into countries (id, name) values ('JAM', 'Jamaica') end
if not exists (select * from countries where id = 'JOR') begin insert into countries (id, name) values ('JOR', 'Jordan') end
if not exists (select * from countries where id = 'JPN') begin insert into countries (id, name) values ('JPN', 'Japan') end
if not exists (select * from countries where id = 'KEN') begin insert into countries (id, name) values ('KEN', 'Kenya') end
if not exists (select * from countries where id = 'KGZ') begin insert into countries (id, name) values ('KGZ', 'Kyrgyzstan') end
if not exists (select * from countries where id = 'KHM') begin insert into countries (id, name) values ('KHM', 'Cambodia') end
if not exists (select * from countries where id = 'KIR') begin insert into countries (id, name) values ('KIR', 'Kiribati') end
if not exists (select * from countries where id = 'COM') begin insert into countries (id, name) values ('COM', 'Comoros') end
if not exists (select * from countries where id = 'KNA') begin insert into countries (id, name) values ('KNA', 'Saint Kitts and Nevis') end
if not exists (select * from countries where id = 'PRK') begin insert into countries (id, name) values ('PRK', 'Korea, Democratic People''s Republic of') end
if not exists (select * from countries where id = 'KOR') begin insert into countries (id, name) values ('KOR', 'Korea, Republic of') end
if not exists (select * from countries where id = 'KWT') begin insert into countries (id, name) values ('KWT', 'Kuwait') end
if not exists (select * from countries where id = 'CYM') begin insert into countries (id, name) values ('CYM', 'Cayman Islands') end
if not exists (select * from countries where id = 'KAZ') begin insert into countries (id, name) values ('KAZ', 'Kazakhstan') end
if not exists (select * from countries where id = 'LAO') begin insert into countries (id, name) values ('LAO', 'Lao People''s Democratic Republic') end
if not exists (select * from countries where id = 'LBN') begin insert into countries (id, name) values ('LBN', 'Lebanon') end
if not exists (select * from countries where id = 'LCA') begin insert into countries (id, name) values ('LCA', 'Saint Lucia') end
if not exists (select * from countries where id = 'LIE') begin insert into countries (id, name) values ('LIE', 'Liechtenstein') end
if not exists (select * from countries where id = 'LKA') begin insert into countries (id, name) values ('LKA', 'Sri Lanka') end
if not exists (select * from countries where id = 'LBR') begin insert into countries (id, name) values ('LBR', 'Liberia') end
if not exists (select * from countries where id = 'LSO') begin insert into countries (id, name) values ('LSO', 'Lesotho') end
if not exists (select * from countries where id = 'LTU') begin insert into countries (id, name) values ('LTU', 'Lithuania') end
if not exists (select * from countries where id = 'LUX') begin insert into countries (id, name) values ('LUX', 'Luxembourg') end
if not exists (select * from countries where id = 'LVA') begin insert into countries (id, name) values ('LVA', 'Latvia') end
if not exists (select * from countries where id = 'LBY') begin insert into countries (id, name) values ('LBY', 'Libyan Arab Jamahiriya') end
if not exists (select * from countries where id = 'MAR') begin insert into countries (id, name) values ('MAR', 'Morocco') end
if not exists (select * from countries where id = 'MCO') begin insert into countries (id, name) values ('MCO', 'Monaco') end
if not exists (select * from countries where id = 'MDA') begin insert into countries (id, name) values ('MDA', 'Moldova, Republic of') end
if not exists (select * from countries where id = 'MDG') begin insert into countries (id, name) values ('MDG', 'Madagascar') end
if not exists (select * from countries where id = 'MHL') begin insert into countries (id, name) values ('MHL', 'Marshall Islands') end
if not exists (select * from countries where id = 'MKD') begin insert into countries (id, name) values ('MKD', 'Macedonia, the Former Yugoslav Republic of') end
if not exists (select * from countries where id = 'MLI') begin insert into countries (id, name) values ('MLI', 'Mali') end
if not exists (select * from countries where id = 'MMR') begin insert into countries (id, name) values ('MMR', 'Myanmar') end
if not exists (select * from countries where id = 'MNG') begin insert into countries (id, name) values ('MNG', 'Mongolia') end
if not exists (select * from countries where id = 'MAC') begin insert into countries (id, name) values ('MAC', 'Macao') end
if not exists (select * from countries where id = 'MNP') begin insert into countries (id, name) values ('MNP', 'Northern Mariana Islands') end
if not exists (select * from countries where id = 'MTQ') begin insert into countries (id, name) values ('MTQ', 'Martinique') end
if not exists (select * from countries where id = 'MRT') begin insert into countries (id, name) values ('MRT', 'Mauritania') end
if not exists (select * from countries where id = 'MSR') begin insert into countries (id, name) values ('MSR', 'Montserrat') end
if not exists (select * from countries where id = 'MLT') begin insert into countries (id, name) values ('MLT', 'Malta') end
if not exists (select * from countries where id = 'MUS') begin insert into countries (id, name) values ('MUS', 'Mauritius') end
if not exists (select * from countries where id = 'MDV') begin insert into countries (id, name) values ('MDV', 'Maldives') end
if not exists (select * from countries where id = 'MWI') begin insert into countries (id, name) values ('MWI', 'Malawi') end
if not exists (select * from countries where id = 'MEX') begin insert into countries (id, name) values ('MEX', 'Mexico') end
if not exists (select * from countries where id = 'MYS') begin insert into countries (id, name) values ('MYS', 'Malaysia') end
if not exists (select * from countries where id = 'MOZ') begin insert into countries (id, name) values ('MOZ', 'Mozambique') end
if not exists (select * from countries where id = 'NAM') begin insert into countries (id, name) values ('NAM', 'Namibia') end
if not exists (select * from countries where id = 'NCL') begin insert into countries (id, name) values ('NCL', 'New Caledonia') end
if not exists (select * from countries where id = 'NER') begin insert into countries (id, name) values ('NER', 'Niger') end
if not exists (select * from countries where id = 'NFK') begin insert into countries (id, name) values ('NFK', 'Norfolk Island') end
if not exists (select * from countries where id = 'NGA') begin insert into countries (id, name) values ('NGA', 'Nigeria') end
if not exists (select * from countries where id = 'NIC') begin insert into countries (id, name) values ('NIC', 'Nicaragua') end
if not exists (select * from countries where id = 'NLD') begin insert into countries (id, name) values ('NLD', 'Netherlands') end
if not exists (select * from countries where id = 'NOR') begin insert into countries (id, name) values ('NOR', 'Norway') end
if not exists (select * from countries where id = 'NPL') begin insert into countries (id, name) values ('NPL', 'Nepal') end
if not exists (select * from countries where id = 'NRU') begin insert into countries (id, name) values ('NRU', 'Nauru') end
if not exists (select * from countries where id = 'NIU') begin insert into countries (id, name) values ('NIU', 'Niue') end
if not exists (select * from countries where id = 'NZL') begin insert into countries (id, name) values ('NZL', 'New Zealand') end
if not exists (select * from countries where id = 'OMN') begin insert into countries (id, name) values ('OMN', 'Oman') end
if not exists (select * from countries where id = 'PAN') begin insert into countries (id, name) values ('PAN', 'Panama') end
if not exists (select * from countries where id = 'PER') begin insert into countries (id, name) values ('PER', 'Peru') end
if not exists (select * from countries where id = 'PYF') begin insert into countries (id, name) values ('PYF', 'French Polynesia') end
if not exists (select * from countries where id = 'PNG') begin insert into countries (id, name) values ('PNG', 'Papua New Guinea') end
if not exists (select * from countries where id = 'PHL') begin insert into countries (id, name) values ('PHL', 'Philippines') end
if not exists (select * from countries where id = 'PAK') begin insert into countries (id, name) values ('PAK', 'Pakistan') end
if not exists (select * from countries where id = 'POL') begin insert into countries (id, name) values ('POL', 'Poland') end
if not exists (select * from countries where id = 'SPM') begin insert into countries (id, name) values ('SPM', 'Saint Pierre and Miquelon') end
if not exists (select * from countries where id = 'PCN') begin insert into countries (id, name) values ('PCN', 'Pitcairn') end
if not exists (select * from countries where id = 'PRI') begin insert into countries (id, name) values ('PRI', 'Puerto Rico') end
if not exists (select * from countries where id = 'PRT') begin insert into countries (id, name) values ('PRT', 'Portugal') end
if not exists (select * from countries where id = 'PLW') begin insert into countries (id, name) values ('PLW', 'Palau') end
if not exists (select * from countries where id = 'PRY') begin insert into countries (id, name) values ('PRY', 'Paraguay') end
if not exists (select * from countries where id = 'QAT') begin insert into countries (id, name) values ('QAT', 'Qatar') end
if not exists (select * from countries where id = 'REU') begin insert into countries (id, name) values ('REU', 'Reunion') end
if not exists (select * from countries where id = 'ROM') begin insert into countries (id, name) values ('ROM', 'Romania') end
if not exists (select * from countries where id = 'RUS') begin insert into countries (id, name) values ('RUS', 'Russian Federation') end
if not exists (select * from countries where id = 'RWA') begin insert into countries (id, name) values ('RWA', 'Rwanda') end
if not exists (select * from countries where id = 'SAU') begin insert into countries (id, name) values ('SAU', 'Saudi Arabia') end
if not exists (select * from countries where id = 'SLB') begin insert into countries (id, name) values ('SLB', 'Solomon Islands') end
if not exists (select * from countries where id = 'SYC') begin insert into countries (id, name) values ('SYC', 'Seychelles') end
if not exists (select * from countries where id = 'SDN') begin insert into countries (id, name) values ('SDN', 'Sudan') end
if not exists (select * from countries where id = 'SWE') begin insert into countries (id, name) values ('SWE', 'Sweden') end
if not exists (select * from countries where id = 'SGP') begin insert into countries (id, name) values ('SGP', 'Singapore') end
if not exists (select * from countries where id = 'SHN') begin insert into countries (id, name) values ('SHN', 'Saint Helena') end
if not exists (select * from countries where id = 'SVN') begin insert into countries (id, name) values ('SVN', 'Slovenia') end
if not exists (select * from countries where id = 'SJM') begin insert into countries (id, name) values ('SJM', 'Svalbard and Jan Mayen') end
if not exists (select * from countries where id = 'SVK') begin insert into countries (id, name) values ('SVK', 'Slovakia') end
if not exists (select * from countries where id = 'SLE') begin insert into countries (id, name) values ('SLE', 'Sierra Leone') end
if not exists (select * from countries where id = 'SMR') begin insert into countries (id, name) values ('SMR', 'San Marino') end
if not exists (select * from countries where id = 'SEN') begin insert into countries (id, name) values ('SEN', 'Senegal') end
if not exists (select * from countries where id = 'SOM') begin insert into countries (id, name) values ('SOM', 'Somalia') end
if not exists (select * from countries where id = 'SUR') begin insert into countries (id, name) values ('SUR', 'Suriname') end
if not exists (select * from countries where id = 'STP') begin insert into countries (id, name) values ('STP', 'Sao Tome and Principe') end
if not exists (select * from countries where id = 'SLV') begin insert into countries (id, name) values ('SLV', 'El Salvador') end
if not exists (select * from countries where id = 'SYR') begin insert into countries (id, name) values ('SYR', 'Syrian Arab Republic') end
if not exists (select * from countries where id = 'SWZ') begin insert into countries (id, name) values ('SWZ', 'Swaziland') end
if not exists (select * from countries where id = 'TCA') begin insert into countries (id, name) values ('TCA', 'Turks and Caicos Islands') end
if not exists (select * from countries where id = 'TCD') begin insert into countries (id, name) values ('TCD', 'Chad') end
if not exists (select * from countries where id = 'TGO') begin insert into countries (id, name) values ('TGO', 'Togo') end
if not exists (select * from countries where id = 'THA') begin insert into countries (id, name) values ('THA', 'Thailand') end
if not exists (select * from countries where id = 'TJK') begin insert into countries (id, name) values ('TJK', 'Tajikistan') end
if not exists (select * from countries where id = 'TKL') begin insert into countries (id, name) values ('TKL', 'Tokelau') end
if not exists (select * from countries where id = 'TKM') begin insert into countries (id, name) values ('TKM', 'Turkmenistan') end
if not exists (select * from countries where id = 'TUN') begin insert into countries (id, name) values ('TUN', 'Tunisia') end
if not exists (select * from countries where id = 'TON') begin insert into countries (id, name) values ('TON', 'Tonga') end
if not exists (select * from countries where id = 'TUR') begin insert into countries (id, name) values ('TUR', 'Turkey') end
if not exists (select * from countries where id = 'TTO') begin insert into countries (id, name) values ('TTO', 'Trinidad and Tobago') end
if not exists (select * from countries where id = 'TUV') begin insert into countries (id, name) values ('TUV', 'Tuvalu') end
if not exists (select * from countries where id = 'TWN') begin insert into countries (id, name) values ('TWN', 'Taiwan, Province of China') end
if not exists (select * from countries where id = 'TZA') begin insert into countries (id, name) values ('TZA', 'Tanzania, United Republic of') end
if not exists (select * from countries where id = 'UKR') begin insert into countries (id, name) values ('UKR', 'Ukraine') end
if not exists (select * from countries where id = 'UGA') begin insert into countries (id, name) values ('UGA', 'Uganda') end
if not exists (select * from countries where id = 'USA') begin insert into countries (id, name) values ('USA', 'United States') end
if not exists (select * from countries where id = 'URY') begin insert into countries (id, name) values ('URY', 'Uruguay') end
if not exists (select * from countries where id = 'UZB') begin insert into countries (id, name) values ('UZB', 'Uzbekistan') end
if not exists (select * from countries where id = 'VAT') begin insert into countries (id, name) values ('VAT', 'Holy See (Vatican City State)') end
if not exists (select * from countries where id = 'VCT') begin insert into countries (id, name) values ('VCT', 'Saint Vincent and the Grenadines') end
if not exists (select * from countries where id = 'VEN') begin insert into countries (id, name) values ('VEN', 'Venezuela') end
if not exists (select * from countries where id = 'VGB') begin insert into countries (id, name) values ('VGB', 'Virgin Islands, British') end
if not exists (select * from countries where id = 'VIR') begin insert into countries (id, name) values ('VIR', 'Virgin Islands, U.s.') end
if not exists (select * from countries where id = 'VNM') begin insert into countries (id, name) values ('VNM', 'Viet Nam') end
if not exists (select * from countries where id = 'VUT') begin insert into countries (id, name) values ('VUT', 'Vanuatu') end
if not exists (select * from countries where id = 'WLF') begin insert into countries (id, name) values ('WLF', 'Wallis and Futuna') end
if not exists (select * from countries where id = 'WSM') begin insert into countries (id, name) values ('WSM', 'Samoa') end
if not exists (select * from countries where id = 'YEM') begin insert into countries (id, name) values ('YEM', 'Yemen') end
if not exists (select * from countries where id = 'ZAF') begin insert into countries (id, name) values ('ZAF', 'South Africa') end
if not exists (select * from countries where id = 'ZMB') begin insert into countries (id, name) values ('ZMB', 'Zambia') end
if not exists (select * from countries where id = 'ZWE') begin insert into countries (id, name) values ('ZWE', 'Zimbabwe') end

/*
	Values for communication options
*/
if not exists (select * from CommunicationOptions where id = 'AS') 
begin
	insert into CommunicationOptions(Id, Name, Description, RequiresAssistant)
	values ('AS', 'Assistant', 'Send all communications to assistant', 1)
end

if not exists (select * from CommunicationOptions where id = 'CA')
begin
	insert into CommunicationOptions(Id, Name, Description, RequiresAssistant)
	values('CA', 'Carbon Copy Assistant', 'CC all communications to assistant', 1)
end

if not exists (select * from CommunicationOptions where id = 'DR')
begin
	insert into CommunicationOptions(Id, Name, Description, RequiresAssistant)
	values('DR', 'Directly', 'Contact Directly', 0)
end


/*
	Values for static notification methods/types
*/
if not exists (select * from NotificationMethods where id = 'E')
begin
	insert into NotificationMethods(Id, Name)
	values ('E', 'EMail')
end

if not exists (select * from NotificationMethods where id = 'P')
begin
	insert into NotificationMethods(Id, Name)
	values ('P', 'Phone')
end

if not exists (select * from NotificationMethods where id = 'L')
begin
	insert into NotificationMethods(Id, Name)
	values ('L', 'Letter')
end

if not exists (select * from NotificationTypes where id = 'IL')
begin
	insert into NotificationTypes(Id, Name)
	values ('IL', 'Invitation Letter')
end

if not exists (select * from NotificationTypes where id = 'RR')
begin
	insert into NotificationTypes(Id, Name)
	values ('RR', 'Registration Reminders')
end

if not exists (select * from NotificationTypes where id = 'HR')
begin
	insert into NotificationTypes(Id, Name)
	values ('HR', 'Hotel Reminder')
end

if not exists (select * from NotificationTypes where id = 'PR')
begin
	insert into NotificationTypes(Id, Name)
	values ('PR', 'Payment Reminder')
end

if not exists (select * from NotificationTypes where id = 'AL')
begin
	insert into NotificationTypes(Id, Name)
	values ('AL', 'Acceptance Letter')
end

if not exists (select * from NotificationTypes where id = 'AC')
begin
	insert into NotificationTypes(Id, Name, Display)
	values ('AC', 'Application Confirmation', 0)
end

if not exists (select * from NotificationTypes where id = 'OT')
begin
	insert into NotificationTypes(Id, Name)
	values ('OT', 'Other')
end

if not exists (select * from roomtypes where name = 'Deluxe Ocean View Corner Room with Balcony')  
begin  insert into roomtypes (name) values ('Deluxe Ocean View Corner Room with Balcony')  end
if not exists (select * from roomtypes where name = 'Deluxe Ocean View Room with Balcony')  
begin  insert into roomtypes (name) values ('Deluxe Ocean View Room with Balcony')  end
if not exists (select * from roomtypes where name = 'Deluxe Ocean View Room No Balcony') 
 begin  insert into roomtypes (name) values ('Deluxe Ocean View Room No Balcony')  end
if not exists (select * from roomtypes where name = 'Inland View Room with Balcony')  
begin  insert into roomtypes (name) values ('Inland View Room with Balcony')  end
if not exists (select * from roomtypes where name = 'Inland View Room No Balcony')  
begin  insert into roomtypes (name) values ('Inland View Room No Balcony')  end
if not exists (select * from roomtypes where name = 'Presidential Suite')  
begin  insert into roomtypes (name) values ('Presidential Suite')  end
if not exists (select * from roomtypes where name = 'Grand Bay Suite')  
begin  insert into roomtypes (name) values ('Grand Bay Suite')  end
if not exists (select * from roomtypes where name = 'Cannery Row Suite')  
begin  insert into roomtypes (name) values ('Cannery Row Suite')  end
if not exists (select * from roomtypes where name = 'Spa Terrace')  
begin  insert into roomtypes (name) values ('Spa Terrace')  end
if not exists (select * from roomtypes where name = 'Executive Suites')  
begin  insert into roomtypes (name) values ('Executive Suites')  end



if not exists (select * from FirmTypes where Name = 'Ag. Production')
begin
	insert into FirmTypes (Name) values ('Ag. Production')
end

if not exists (select * from FirmTypes where Name = 'Processing and Marketing')
begin
	insert into FirmTypes (Name) values ('Processing and Marketing')
end

if not exists (select * from FirmTypes where Name = 'Farm Inputs')
begin
	insert into FirmTypes (Name) values ('Farm Inputs')
end

if not exists (select * from FirmTypes where Name = 'Financial Services')
begin
	insert into FirmTypes (Name) values ('Financial Services')
end

if not exists (select * from FirmTypes where Name = 'Other Farm Services')
begin
	insert into FirmTypes (Name) values ('Other Farm Services')
end

if not exists (select * from FirmTypes where Name = 'Restaurants and Food Service')
begin
	insert into FirmTypes (Name) values ('Restaurants and Food Service')
end

if not exists (select * from FirmTypes where Name = 'Retail')
begin
	insert into FirmTypes (Name) values ('Retail')
end