SELECT * FROM Orders
SELECT * FROM [Order Details]
SELECT * FROM Products
GO

--Berechne in einer Funktion die Summe des Warenwertes eines Produktes im Lager.

CREATE OR ALTER FUNCTION fnGesamtwarenwert(@ProductId INT)
RETURNS DECIMAL(18,2)
AS
BEGIN
	DECLARE @ret int;
	SELECT @ret = Unitprice * UnitsInStock FROM Products
	WHERE ProductID = @ProductId
	RETURN @ret
END
GO

--CREATE, FUNCTION , RETURNS , BEGIN, END

SELECT p.ProductID, p.ProductName, p.UnitPrice, p.UnitsInStock, dbo.fnGesamtwarenwert(1) AS "Gesamtwert im Lager" FROM Products p
WHERE ProductID = 1

-- AUFGABE:
-- Eine Funktion schreiben, die den Anzahl 
-- der Untergeordneten vom angegebenen Mitarbeiter 
-- zurück gibt.

SELECT * FROM Employees WHERE ReportsTo = 2
SELECT COUNT(EmployeeID) FROM Employees WHERE ReportsTo = 2
-- 2022-09-07

GO
CREATE FUNCTION fnAnzahlUntergeordnete(@VorgesetzeId int)  
RETURNS int   
AS BEGIN 
     RETURN (SELECT COUNT(EmployeeID) 
		FROM Employees WHERE ReportsTo = @VorgesetzeId)
		-- Implizite Konvertierung ()
END;
GO