/*1. A Custom access control trigger. 
Disallow edit (update/insert/delete) of tables Transaction and Customer from 18.30 till 08:00.*/
CREATE TRIGGER Tr_TimeLimitedChanges_Transactions ON Transactions
FOR INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @CurrentTime TIME = CAST(GETDATE() AS TIME);
    DECLARE @StartTime TIME = '18:30:00', @EndTime TIME = '08:00:00';

    IF @CurrentTime > @StartTime OR @CurrentTime < @EndTime
    BEGIN
        RAISERROR('Transactions records cannot be editted from 18.30 till 08.00', 16, 1);
        ROLLBACK TRANSACTION;
    END
END

GO
CREATE TRIGGER Tr_TimeLimitedChanges_Customers ON Customers
FOR INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @CurrentTime TIME = CAST(GETDATE() AS TIME);
    DECLARE @StartTime TIME = '18:30:00', @EndTime TIME = '08:00:00';

    IF @CurrentTime > @StartTime OR @CurrentTime < @EndTime
    BEGIN
        RAISERROR('Customers records cannot be editted from 18.30 till 08.00', 16, 1);
        ROLLBACK TRANSACTION;
    END
END

GO
/*2. A Trigger that will perform validation of some business rule (constraint). 
Implement a trigger that will check that customer does not have more then 5 transactions per day.*/
CREATE TRIGGER Tr_TransactionsPerDay ON Transactions
FOR INSERT
AS
BEGIN
    IF (SELECT COUNT(*) FROM Transactions 
        WHERE CustomerId=(SELECT CustomerId FROM Inserted)
        AND Date = (SELECT Date FROM Inserted)) > 5
    BEGIN
        RAISERROR('Customer cannot have more than 5 transactions per day', 16, 1);
        ROLLBACK TRANSACTION;
    END
END

GO
/*3. A trigger for data logging. 
In the "Customer" table, introduce a new column named "TotalTransactionsAmount" to capture the cumulative sum of all transactions 
associated with the customer [sum(Transaction.total)]. Implement a trigger that will maintain this column up to date.*/
CREATE TRIGGER Tr_UpdateTotalTransactionAmount ON Transactions
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @CustomerId INT = (SELECT CustomerId FROM Inserted UNION ALL SELECT CustomerId FROM Deleted);
    
    UPDATE Customers
    SET TotalTransactionsAmount = (SELECT SUM(Total) FROM Transactions WHERE CustomerId=@CustomerId)
    WHERE Id=@CustomerId;
END
