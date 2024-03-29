create login db_login1 with password = '12345'

go 

use [C:\USERS\BOTIR\ONEDRIVE\РАБОЧИЙ СТОЛ\DBSD_17286_16777_17037_\DBSD_17037_16777_17286\DBSD_17037_16777_17286\DATA\MACRO1.MDF]


go

go 
create user user1 for login db_login1

go
-- Create the user 'macro_user1' for the login 'db_login1'
CREATE USER macro_user1 FOR LOGIN db_login1;

-- Grant SELECT permission
GRANT SELECT ON Departments TO user1;
GRANT SELECT ON Employees TO user1;
GRANT SELECT ON Persons TO user1;

-- Grant UPDATE permission
GRANT UPDATE ON Departments TO user1;
GRANT UPDATE ON Employees TO user1;
GRANT UPDATE ON Persons TO user1;

-- Grant INSERT permission
GRANT INSERT ON Departments TO user1;
GRANT INSERT ON Employees TO user1;
GRANT INSERT ON Persons TO user1;

-- Grant DELETE permission
GRANT DELETE ON Departments TO user1;
GRANT DELETE ON Employees TO user1;
GRANT DELETE ON Persons TO user1;