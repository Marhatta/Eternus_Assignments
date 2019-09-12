use MyECommerce;


CREATE TABLE SubCategory(
	SubCategoryId int PRIMARY KEY ,
	Name varchar(30) NOT NULL
);

CREATE TABLE Category(
	CategoryId int PRIMARY KEY,
	Name varchar(30) NOT NULL,
	SubCategoryId int NOT NULL,
	CONSTRAINT fk_Category FOREIGN KEY(SubCategoryId)
	REFERENCES dbo.SubCategory(SubCategoryId)
);



CREATE TABLE Products(
	ProductId int PRIMARY KEY,
	Name varchar(30) NOT NULL,
	Picture image,
	Detail varchar(30),
	Quantity int NOT NULL ,
	Size varchar(10) NOT NULL,
	Color varchar(10) NOT NULL,
	Discount float,
	CategoryId int NOT NULL,
	SupplierId int NOT NULL,
	CONSTRAINT fk_Products_Category FOREIGN KEY(CategoryId)
	REFERENCES Category(CategoryId)
	)
	

CREATE TABLE Suppliers(
	SupplierId int PRIMARY KEY,
	Name varchar(30) NOT NULL,
	Address varchar(30) NOT NULL,
	City varchar(20) NOT NULL,
	State varchar(20) NOT NULL,
	Country varchar(20) NOT NULL,
	Contact int NOT NULL CHECK (Contact not like '%[^0-9]%'),
	CustomerId int NOT NULL,
	ProductId int NOT NULL,
	CONSTRAINT fk_Suppliers FOREIGN KEY(ProductId)
	REFERENCES Products(ProductId)
);






CREATE TABLE Customer(
	CustomerId int PRIMARY KEY NOT NULL,
	FirstName varchar(30) NOT NULL,
	LastName varchar(30) NOT NULL,
	City varchar(20) NOT NULL,
	State varchar(20) NOT NULL,
	Pin int NOT NULL,
	Country varchar(20) NOT NULL,
	Address varchar(50) NOT NULL,
	Contact int NOT NULL  CHECK (Contact not like '%[^0-9]%'),
	Email varchar(30) NOT NULL,
	Password varchar(30) NOT NULL
);

CREATE TABLE Orders(
	OrderId int PRIMARY KEY,
	CustomerId int NOT NULL,
	PaymentId int NOT NULL,
	OrderDate date NOT NULL,
	ShipperId int NOT NULL,
	Shipped tinyint NOT NULL,
	Delivered tinyint NOT NULL,
	CONSTRAINT fk_Orders_Customer FOREIGN KEY(CustomerId)
	REFERENCES Customer(CustomerId),
	CONSTRAINT fk_Orders_Shipper FOREIGN KEY(ShipperId)
	REFERENCES Shipper(ShipperId)
);



CREATE TABLE OrderDetails(
	OrderId int PRIMARY KEY,
	ProductId int NOT NULL,
	TrackingId varchar(20) NOT NULL,
	Tax float NOT NULL,
	Quantity int NOT NULL,
	PaymentType varchar(10) NOT NULL,
	PaymentReceived tinyInt NOT NULL,
	Refund int,
	totalAmount int NOT NULL,
	CustomerId int NOT NULL,
	CONSTRAINT fk_OrderDetails FOREIGN KEY(CustomerId)
	REFERENCES Customer(CustomerId)	
);


CREATE TABLE Shipper(
	ShipperId int PRIMARY KEY,
	Name varchar(30) NOT NULL,
	Contact int NOT NULL CHECK (Contact not like '%[^0-9]%')
);



CREATE table BillingAddress(
	CustomerId int PRIMARY KEY,
	City varchar(30) NOT NULL,
	State varchar(20) NOT NULL,
	Pin int NOT NULL,
	Country varchar(30) NOT NULL,
	Address1 varchar(50),
	Address2 varchar(50)
);

CREATE TABLE Cart(
	ProductId int PRIMARY KEY,
	Quantity int NOT NULL,
	CustomerId int NOT NULL,
	CONSTRAINT fk_Cart_Customer FOREIGN KEY(CustomerId)
	REFERENCES Customer(CustomerId)
);

CREATE TABLE Payments(
	CustomerId int PRIMARY KEY,
	Type varchar(20) NOT NULL,
	CardNo int NOT NULL,
	Expiry varchar(10) NOT NULL
)


----Inserting Values=======================================================================

----Customer table
insert into Customer values(123,'vishal','marhatta','jammu','j&k',180013,'India','H No 377',123456789,'asd@gmail.com','password');
insert into Customer values(124,'rohit','sharma','mumbai','mah',180213,'India','H No 323',123478789,'art@gmail.com','password');
insert into Customer values(125,'virat','kohli','jammu','mah',180413,'India','H No 37',123444789,'afg@gmail.com','password');

----Cart table
insert into Cart values(222,2,123);
insert into Cart values(224,1,123);
insert into Cart values(256,3,124);


----Shipper Table
insert into Shipper values(300,'Ship1',1234554478);
insert into Shipper values(301,'Ship2',1234544478);
insert into Shipper values(302,'Ship3',1234512475);

----Orders table
insert into Orders values(403,123,400,'2014/10/22',300,1,0);
insert into Orders values(404,123,392,'2019/03/12',300,1,0);
insert into Orders values(405,124,440,'2019/04/06',302,1,1);

----Order Details Table
insert into OrderDetails values(403,222,234234367,44,1,'CreditCard',1,0,123);
insert into OrderDetails values(404,232,234764324,67,2,'Debit Card',1,0,123);
insert into OrderDetails values(405,342,234554324,55,1,'Debit Card',1,0,125);

----Billing Address
insert into BillingAddress values(123,'jammu','j&k',180013,'India','h no 377',null)
insert into BillingAddress values(124,'jammu','j&k',180013,'India','h no 456 sec 2','h no 555 sec 1')
insert into BillingAddress values(125,'pune','mah',411052,'India','906/A',null)

----SubCategory Table
insert into SubCategory values(9,'tShirt')
insert into SubCategory values(8,'jeans')
insert into SubCategory values(7,'Mobiles')

----Category Table
insert into Category values(1,'UpperWear',9);
insert into Category values(2,'Electronics',7);
insert into Category values(3,'LowerWear',8);


----Products table
insert  into Products values(222,'t shirt',null,'details about t shirt',12,'M','Red',10,1,500);
insert  into Products values(224,'Lenovo A6',null,'details about lenovo',12,'6 inch','Black',null,2,501);
insert  into Products values(256,'huawei nova 3',null,'details about Huawei',32,'6 inch','Red',20,1,502);
insert  into Products values(258,'n 72',null,'details about nokia',32,1234,'4 inch','black',20,1,502);


----Suppliers table
insert into Suppliers values(500,'Supp1','Sup1Add','Pune','Mah','India',123345768,123,222)
insert into Suppliers values(501,'Supp2','Sup2Add','Pune','Mah','India',123335768,123,224)
insert into Suppliers values(502,'Supp3','Sup3Add','mumbai','Mah','India',123367568,124,256)



----========================================================================================================



----Customers with multiple orders using joins
Select FirstName,LastName,c.CustomerId,OrderId from Orders o INNER JOIN Customer c ON o.CustomerId = c.CustomerId 
where o.CustomerId IN (select CustomerId from Orders group by CustomerId having count(*)>1)

----Product details present in cart
select * from Cart c INNER JOIN Products p ON c.ProductId = p.ProductId 

----Distinct customers who has made multiple orders
select  FirstName,LastName from Customer where CustomerId in(select CustomerId from Orders group by CustomerId having count(*)>1)

----Distinct orders by multiple customers
Select * from Orders o,Customer c where o.CustomerId = c .CustomerId;

----============================================================================================================

--Function to apply discount=============================================================
--create coupons table


drop table coupons;
CREATE TABLE coupons(
Id int PRIMARY KEY IDENTITY(1,1),
name varchar(30),
validFrom DATE,
validUpto DATE
)

select * from coupons;

insert into coupons values('FLAT20','2019-06-22','2019-06-30');

--function to apply discount
print dbo.udfApplyDiscount(13040,'FLAT20')

--spRefund has one argument called orderiD
dbo.spRefund 403

--Cursor to check total order amount=====================================================

DECLARE @Amount float
SET @Amount = 0; 
DECLARE @TotalAmount float
SET @TotalAmount = 0;

DECLARE CheckOrderAmount CURSOR FOR

SELECT p.Price*o.Quantity as TotalPrice from Products p , OrderDetails o  where p.ProductId IN (SELECT ProductId from OrderDetails) ;

OPEN CheckOrderAmount

FETCH NEXT FROM CheckOrderAmount     
INTO @Amount

WHILE @@FETCH_STATUS = 0    
BEGIN    
     SET @TotalAmount += @Amount;
     FETCH NEXT FROM CheckOrderAmount INTO @Amount   
END     
--PRINT 'Total Amount spent is : ' + @TotalAmount
if @TotalAmount > 20000 
	BEGIN
		INSERT INTO coupons values('OFF10',DATEADD(MONTH,1,GETDATE()),DATEADD(MONTH,2,GETDATE()))
	END
else PRINT 'OOPS ! Your total spending is not more than 20,000'
CLOSE CheckOrderAmount;   
DEALLOCATE CheckOrderAmount;    



--====================Views for Orders=============================




CREATE VIEW Monthly AS
SELECT DATENAME(month,OrderDate) AS Month ,
COUNT(DISTINCT o.OrderId) AS TotalOrders,
SUM(od.totalAmount) AS TotalAmount
from Orders o,OrderDetails od WHERE o.OrderId = od.OrderId GROUP BY  DATENAME(month,OrderDate) 

select * from Monthly;


--===================================================

CREATE VIEW Yearly AS
SELECT DATENAME(year,OrderDate) AS Year ,
COUNT(DISTINCT o.OrderId) AS TotalOrders,
SUM(od.totalAmount) AS TotalAmount
from Orders o,OrderDetails od WHERE o.OrderId = od.OrderId GROUP BY  DATENAME(year,OrderDate) 

select * from Yearly;

--=====================================================================


CREATE VIEW Daily AS
SELECT DATENAME(day,OrderDate) AS Date ,
COUNT(DISTINCT o.OrderId) AS TotalOrders,
SUM(od.totalAmount) AS TotalAmount
from Orders o,OrderDetails od WHERE o.OrderId = od.OrderId GROUP BY  OrderDate

select * from Daily

--======================================================================

--Trigger-----
CREATE TABLE totalSale(
Date DATE PRIMARY KEY,
Amount float,
CreatedOn DATE,
UpdatedOn DATE
);


DROP TABLE totalSale;


--==============================================================
Delete from OrderDetails where OrderId = 408
insert into OrderDetails values(408,222,234234376,54,1,'CreditCard',1,123);
insert into OrderDetails values(410,272,234765324,67,2,'Debit Card',1,123);

SELECT * FROM totalSale;
SELECT * FROM OrderDetails;
DROP TRIGGER trgTotalSale;

DROP TABLE totalSale;

SELECT Price from Products WHERE ProductId = 222;

