SELECT * FROM Orders
SELECT * FROM [Order Details]
SELECT * FROM Products
GO

--berechne in einer Funktion die Summe des Warenwertes eines Produktes im Lager.

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