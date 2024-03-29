DROP PROCEDURE GetAllEmployees;
DROP PROCEDURE GetEmployeeById;
DROP PROCEDURE InsertEmployee;
DROP PROCEDURE UpdateEmployee;
DROP PROCEDURE DeleteEmployee;
DROP PROCEDURE udpExportEmployeeDataToJson

--get all
go 

CREATE PROCEDURE GetAllEmployees
AS
BEGIN
  SELECT 
    e.Id, e.HireDate, e.HourlyRate, e.IsMarried, e.Photo, 
    e.DepartmentId, d.Name as DepartmentName,
    e.ManagerId, 
    mgr.PersonId as ManagerPersonId, 
    mgrPerson.FirstName as ManagerFirstName, mgrPerson.LastName as ManagerLastName, 
    e.PersonId, p.ContactDetails, p.FirstName, p.LastName
  FROM Employees e
  INNER JOIN Departments d ON e.DepartmentId = d.Id
  LEFT JOIN Employees mgr ON e.ManagerId = mgr.Id
  LEFT JOIN Persons mgrPerson ON mgr.PersonId = mgrPerson.Id -- Join for manager's Person
  INNER JOIN Persons p ON e.PersonId = p.Id; 
END


-- get by id
go 
CREATE PROCEDURE GetEmployeeById
  @Id INT
AS
BEGIN
  SELECT 
    e.Id, e.HireDate, e.HourlyRate, e.IsMarried, e.Photo, 
    e.DepartmentId, d.Name as DepartmentName,
    e.ManagerId, 
    mgr.PersonId as ManagerPersonId, 
    mgrPerson.FirstName as ManagerFirstName, mgrPerson.LastName as ManagerLastName, 
    e.PersonId, p.ContactDetails, p.FirstName, p.LastName
  FROM Employees e
  INNER JOIN Departments d ON e.DepartmentId = d.Id
  LEFT JOIN Employees mgr ON e.ManagerId = mgr.Id
  LEFT JOIN Persons mgrPerson ON mgr.PersonId = mgrPerson.Id -- Join for manager's Person
  INNER JOIN Persons p ON e.PersonId = p.Id
  WHERE e.Id = @Id;
END

-- insert employee
go 

CREATE PROCEDURE InsertEmployee
    @HireDate DATETIME,
    @HourlyRate DECIMAL,
    @isMarried BIT, 
    @Photo VARBINARY(MAX) = NULL, 
    @DepartmentId INT,
    @ManagerId INT, 
    @PersonId INT
AS
BEGIN
    INSERT INTO Employees (HireDate, HourlyRate, IsMarried, Photo, DepartmentId, ManagerId, PersonId)
    VALUES (@HireDate, @HourlyRate, @isMarried, 
            CASE WHEN @Photo IS NOT NULL THEN @Photo ELSE NULL END,
            @DepartmentId, @ManagerId, @PersonId);

    SELECT SCOPE_IDENTITY(); -- Returns the newly inserted Id
END

-- update employee
go

CREATE PROCEDURE UpdateEmployee
    @Id INT,
    @HireDate DATETIME,
    @HourlyRate DECIMAL,
    @isMarried BIT,
    @Photo VARBINARY(MAX), 
    @DepartmentId INT,
    @ManagerId INT, 
    @PersonId INT
AS
BEGIN
    UPDATE Employees 
    SET HireDate = @HireDate,
        HourlyRate = @HourlyRate,
        IsMarried = @isMarried, 
        Photo = CASE WHEN @Photo IS NOT NULL THEN @Photo ELSE Photo END,
        DepartmentId = @DepartmentId,
        ManagerId = @ManagerId, 
        PersonId = @PersonId
    WHERE Id = @Id;
END

-- delete 

go 
CREATE PROCEDURE DeleteEmployee
    @Id INT 
AS
BEGIN
    -- Update employees with this manager to have null manager
    UPDATE Employees SET ManagerId = NULL WHERE ManagerId = @Id;

    DELETE FROM Employees WHERE Id = @Id;
END

--
go
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