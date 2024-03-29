DROP PROCEDURE GetAllEmployees;
DROP PROCEDURE GetEmployeeById;
DROP PROCEDURE InsertEmployee;
DROP PROCEDURE UpdateEmployee;
DROP PROCEDURE DeleteEmployee;


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



-- filter page and sorting
go
CREATE OR ALTER PROCEDURE udpFilterEmployees(
  @FirstName NVARCHAR(200) = NULL,
  @LastName NVARCHAR(200) = NULL,
  @HireDate DATE = NULL,
  @DepartmentName NVARCHAR(200) = NULL, -- Added parameter

  @SortField NVARCHAR(200) = 'Id',
  @SortDesc BIT = 0,

  @Page INT = 1,
  @PageSize INT = 2
)
AS
BEGIN
  DECLARE @SortDir NVARCHAR(10) = 'ASC'
  IF @SortDesc = 1
    SET @SortDir = 'DESC'

  DECLARE @paramsDef NVARCHAR(2000) = '@FirstName NVARCHAR(200), @LastName NVARCHAR(200), @HireDate DATE, @DepartmentName NVARCHAR(200), @PageSize INT, @Page INT' -- Updated parameter list
  
  DECLARE @sql NVARCHAR(MAX) = '
    SELECT 
      e.Id, e.HireDate, e.HourlyRate, e.IsMarried, e.Photo, 
      e.DepartmentId, d.Name as DepartmentName,
      e.ManagerId, 
      mgr.PersonId as ManagerPersonId, 
      mgrPerson.FirstName as ManagerFirstName, mgrPerson.LastName as ManagerLastName, 
      e.PersonId, p.ContactDetails, p.FirstName, p.LastName,
      COUNT(*) OVER () AS TotalRows
    FROM Employees e
    INNER JOIN Departments d ON e.DepartmentId = d.Id
    LEFT JOIN Employees mgr ON e.ManagerId = mgr.Id
    LEFT JOIN Persons mgrPerson ON mgr.PersonId = mgrPerson.Id
    INNER JOIN Persons p ON e.PersonId = p.Id';

  IF @FirstName IS NOT NULL
    SET @sql = @sql + ' WHERE p.FirstName LIKE @FirstName + ''%'''
  IF @LastName IS NOT NULL
    SET @sql = CASE WHEN @FirstName IS NOT NULL THEN @sql + ' AND' ELSE @sql + ' WHERE' END + ' p.LastName LIKE @LastName + ''%'''
  IF @HireDate IS NOT NULL
    SET @sql = CASE WHEN @FirstName IS NOT NULL OR @LastName IS NOT NULL THEN @sql + ' AND' ELSE @sql + ' WHERE' END + ' e.HireDate >= @HireDate'
  IF @DepartmentName IS NOT NULL -- Added condition for DepartmentName filter
    SET @sql = CASE WHEN @FirstName IS NOT NULL OR @LastName IS NOT NULL OR @HireDate IS NOT NULL THEN @sql + ' AND' ELSE @sql + ' WHERE' END + ' d.Name LIKE @DepartmentName + ''%'''

 SET @sql = @sql + '
    ORDER BY ' + @SortField + ' ' + @SortDir + '
    OFFSET @PageSize * @Page ROWS
    FETCH NEXT @PageSize ROWS ONLY';

  EXEC sp_executesql @sql, @paramsDef,
    @FirstName = @FirstName,
    @LastName = @LastName,
    @HireDate = @HireDate,
    @DepartmentName = @DepartmentName, -- Added parameter
    @PageSize = @PageSize,
    @Page = @Page;
END
