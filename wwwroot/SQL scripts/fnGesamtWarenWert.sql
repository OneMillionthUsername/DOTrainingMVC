-----------------------------BEISPIELAUFGABEN-----------------------------

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

-- AUFGABE:
-- Eine Funktion schreiben, die den Anzahl 
-- der Untergeordneten vom angegebenen Mitarbeiter 
-- zurück gibt.

CREATE FUNCTION fnAnzahlUntergeordnete(@VorgesetzeId int)  
RETURNS int   
AS BEGIN 
     RETURN (SELECT COUNT(EmployeeID) 
		FROM Employees WHERE ReportsTo = @VorgesetzeId)
		-- Implizite Konvertierung ()
END
GO

-- Liste alle Lieferanten auf, die aus Frankreich, Italien oder Deutschlang sind.

select * from [dbo].[Suppliers]
where Country = 'France' OR Country = 'Italy' OR Country = 'Germany'

-- Alle Bestellungsadressen ohne Wiederholung ausgeben
select * from [dbo].[Orders]
-- zeigt nur eindeutige EXPLIZITE Datensätze
select distinct ShipAddress from [dbo].[Orders]
order by ShipAddress asc

-- wie oben mit PLZ und Ort
select distinct ShipAddress, ShipPostalCode, ShipCity from [dbo].[Orders]
order by ShipAddress asc

-- Alle Bestellungsadressen
-- [ShipAddress], [ShipPostalCode], [ShipCity]
-- ohne wiederholung und ohne USA aufilsten
select distinct ShipAddress, ShipPostalCode, ShipCity from [dbo].[Orders]
where ShipCountry != 'USA'

-- Liste aller Städte aus der Liferantentabelle sind ohne Wiederholungen auszugeben
select distinct City from [dbo].[Suppliers]
order by City asc --aufsteigend

-- Eine Liste aller Städte ohne Wiederholung, in die verschickt wurde
select distinct ShipCity as Städte from [dbo].[Orders]
order by ShipCity desc --descending absteigend

-- Lieferanten Länder ohne Wiederholung alfabetisch angeordnet auflisten
select distinct Country from [dbo].[Suppliers] order by Country asc

-- Bitte die Adressdaten von der Tabelle [dbo].[Suppliers] als eine einzige Spalte ausgeben! Datensätze mit Null-Werte ausschließen
-- Null Werte durch leeren String ersetzen.
select Address + ' ' + City + ' ' + ISNULL(Region +' ', '') + Country as Adresse from [dbo].[Suppliers] 
where Region IS NOT NULL --!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

-- Einträge der Mitarbeiter Robert und Laura ausgeben
-- Gewünschte Ausgabe: [FirstName], [LastName], [EmployeeID], [City], [PostalCode], [Country]
select [FirstName], [LastName], [EmployeeID], [City], [PostalCode], [Country] from [dbo].[Employees]
where FirstName = 'Laura' OR FirstName = 'Robert'

-- Mitarbeiter-ID, Vorname und Nachname verkettet ausgeben. Ausgaben mehrfacher Leerzeichen verhindern.
-- EmployeeID ist ein int. Müssen wir in string umwandeln.
SELECT convert(nvarchar, EmployeeID) + ' ' + FirstName + ' ' + LastName FROM Employees
-- Verwende die Zeichen von EmployeeID bis MAXIMAL 5
SELECT convert(varchar(5), EmployeeID) + ' ' + FirstName + ' ' + LastName FROM Employees
-- Verwende die Zeichen von EmployeeID und fülle den String bis MAXIMAL 10 Zeichen mit Leerzeichen
SELECT convert(char(10), EmployeeID) + ' ' + FirstName + ' ' + LastName FROM Employees

-- Werte der Produkte im Lager ausgeben
-- Ausgabe: [ProductID],[ProductName],Wert
select [ProductID], [ProductName], [UnitsInStock]*[UnitPrice] as Wert from [dbo].[Products]
order by Wert desc

--Liste aller Kunden (ID, Firmenname, Stadt, Land)
--sortiert nach Land aufsteigend, Stadt absteigend,
--Kontaktname aufsteigend

select * from [dbo].[Customers]
order by [Country] asc, City desc, [ContactName] asc

--Lieferanten Adressen aus USA alphabetisch nach Adress angeordnet auflisten
-- gewünschte Ausgabe: Address, City, Region, PostalCode, Country

select [Address], [City], [Region], [PostalCode], [Country] from [dbo].[Suppliers]
where Country = 'USA'
order by Address asc

-- Prlduktliste (Name, ID) und Preis
-- von allen Produkten die billiger als 20 sind

select [ProductName], [ProductID], [UnitPrice] from [dbo].[Products]
where UnitPrice < 20

-- Produktliste (Name) und Preis von allen Produkten
-- die zw. 15 und 20 (inkl.) kosten

select [ProductName], [UnitPrice] from [dbo].[Products]
where UnitPrice >= 15 AND UnitPrice <= 20

select [ProductName], [UnitPrice] from [dbo].[Products]
where UnitPrice BETWEEN 15 AND 20

-- Alle Mitarbeiter-Einträge mit der ID 4, 6 und 7 ausgeben-- Gewünschte Ausgabe: VornameNachnamen (in einer Spalte), [HomePhone]
select [FirstName]+' '+[LastName] as VornameNachnamen, HomePhone from [dbo].[Employees]
where [EmployeeID] = 4 or [EmployeeID] = 6 or [EmployeeID] = 7

select [FirstName]+' '+[LastName] as VornameNachnamen, HomePhone from [dbo].[Employees]
where [EmployeeID] in (4,6,7) --IN OPERATOR

--Alle Bestellungen außer der ShipVia 1 und 2 ausgeben
--gewünschte Ausgabe: OrderDate, OderId, ShipVia, ShipCountry
--hinweis IN OPERATOR
select OrderDate, OrderID, ShipVia, ShipCountry from [dbo].[Orders]
where ShipVia in (3)

select OrderDate, OrderID, ShipVia, ShipCountry from [dbo].[Orders]
where ShipVia not in (1, 2)

select OrderDate, OrderID, ShipVia, ShipCountry from [dbo].[Orders]
where ShipVia != 1 and ShipVia != 2 --HIER AND statt OR

select OrderDate, OrderID, ShipVia, ShipCountry from [dbo].[Orders]
where not ShipVia != 1 or not ShipVia != 2 --HIER OR statt AND

-- Anzahl der Mitarbeiter, die 1993 angestellt wurden
select count(employeeID) Anzahl from Employees
where YEAR(HireDate) = 1993 -- Year bezieht sich auf die Spalte und gibt das Jahr zurück.

--Anzahl der Bestellungen vom 2 Tag der Monate vom Jahr 1997
--Gewünschte Ausgabe: Anzahl
select * from orders
select count(OrderID) Anzahl from Orders
where YEAR(orderdate) = 1997 and day(orderdate) = 2

-- Anzahl der Bestellungen an Montage vom 1996
select count(orderID) Anzahl from orders
where DATEPART(WEEKDAY, OrderDate) = 1 and year(OrderDate) = 1996

--SELECT DATEPART ([WEEKDAY], '11.5.2022')
-- 3 = Mittwoch, 1 = Montag, usw.
select count(orderID) Anzahl from orders
where DATENAME(WEEKDAY, OrderDate) = 'Montag' and year(OrderDate) = 1996

--Alle Mitarbeiter auflisten, die zwischen 1952 und 1963 geboren sind
select * from Employees
where year(BirthDate) between 1952 and 1963
order by BirthDate asc

--Anzahl der Bestellungen pro Monat
select year(orderdate) Jahr, month(orderdate) Monat, count(orderid) Bestellungen from Orders
group by year(orderdate), MONTH(orderdate)
order by year(orderdate), MONTH(orderdate)