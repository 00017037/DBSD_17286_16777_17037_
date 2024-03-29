create or alter procedure udpExportEmployeeDataToJson(
  @Id INT,
  @FirstName nvarchar(200),
  @LastName nvarchar(200),
  @HireDate DateTime,
  @IsMarried BIT,
  @ManagerFirstName nvarchar(200),
  @ManagerLastName nvarchar(200)
) as 
begin
  SELECT 
    e.Id "EmployeeId", e.HireDate "HireDate", e.IsMarried "IsMarried", e.HourlyRate "HourlyRate", 
    d.Name "DepartmentName",  
    mgrPerson.FirstName "ManagerFirstName", mgrPerson.LastName "ManagerLastName", 
    p.FirstName "FirstName", p.LastName "LastName"
  FROM Employees e
  INNER JOIN Departments d ON e.DepartmentId = d.Id
  LEFT JOIN Employees mgr ON e.ManagerId = mgr.Id
  LEFT JOIN Persons mgrPerson ON mgr.PersonId = mgrPerson.Id -- Join for manager's Person
  INNER JOIN Persons p ON e.PersonId = p.Id
  for json auto, root('EmployeesData')
end
--test
exec udpExportEmployeeDataToJson 
  @Id = NULL, 
  @FirstName = NULL, 
  @LastName = NULL, 
  @HireDate = NULL, 
  @IsMarried = NULL, 
  @ManagerFirstName = NULL, 
  @ManagerLastName = NULL;

-- Export to xml --
go
create or alter procedure udpExportEmployeeDataToXml(
  @Id INT,
  @FirstName nvarchar(200),
  @LastName nvarchar(200),
  @HireDate DateTime,
  @IsMarried BIT,
  @ManagerFirstName nvarchar(200),
  @ManagerLastName nvarchar(200)
) as 
begin
  SELECT 
    e.Id "EmployeeId", e.HireDate "HireDate", e.IsMarried "IsMarried", e.HourlyRate "HourlyRate", 
    d.Name "DepartmentName",  
    mgrPerson.FirstName "ManagerFirstName", mgrPerson.LastName "ManagerLastName", 
    p.FirstName "FirstName", p.LastName "LastName"
  FROM Employees e
  INNER JOIN Departments d ON e.DepartmentId = d.Id
  LEFT JOIN Employees mgr ON e.ManagerId = mgr.Id
  LEFT JOIN Persons mgrPerson ON mgr.PersonId = mgrPerson.Id 
  INNER JOIN Persons p ON e.PersonId = p.Id
  for xml auto, root('EmployeesData')
end
--test
exec udpExportEmployeeDataToXml
  @Id = NULL, 
  @FirstName = NULL, 
  @LastName = NULL, 
  @HireDate = NULL, 
  @IsMarried = NULL, 
  @ManagerFirstName = NULL, 
  @ManagerLastName = NULL;
