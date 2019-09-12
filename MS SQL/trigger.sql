use MyECommerce;


--======================================================================

CREATE TABLE totalSale(
SaleDate DATE,
Amount float,
UpdatedOn DATE
);


--Trigger to find total amount of sale
ALTER TRIGGER trgTotalSale
ON OrderDetails
AFTER INSERT,UPDATE
AS
begin 	
declare @check int
declare @price float
declare @refund_status int
declare @orderDate DATE
declare @refundStatusUpdated int

set @refundStatusUpdated = 0
set @price  = (select p.Price*i.Quantity from Products p ,inserted i where p.ProductId = i.ProductId)
set @orderDate = (select o.OrderDate from Orders o ,inserted i where o.OrderId = i.OrderId)
set @check = (select distinct 0 from totalSale where SaleDate = @orderDate)
set @refund_status = (select i.Refund from Products p , inserted i where p.ProductId = i.ProductId)

	IF(@check=0)
		BEGIN
		IF(@refund_status = 1)
			UPDATE totalSale SET Amount = (Amount - @price) , UpdatedOn = convert(date,GETDATE()) WHERE SaleDate = @orderDate
		ELSE 
			UPDATE totalSale SET Amount = (Amount + @price) , UpdatedOn = convert(date,GETDATE()) WHERE SaleDate = @orderDate
		END
	ELSE
		INSERT INTO totalSale(
			SaleDate,
			Amount,
			UpdatedOn
		)
		values( 
			@orderDate,
			@price,
			GETDATE()
		)
		
end
--============================================================================

--Test values
Delete from OrderDetails where OrderId = 405
Delete from OrderDetails where OrderId = 404
update OrderDetails set Refund = 1 where OrderId = 405
update OrderDetails set PaymentType='Debit Card' where OrderId = 405
update OrderDetails set PaymentType='Debit Card' where OrderId = 404
insert into OrderDetails values(405,222,234234376,54,2,'CreditCard',1,0,123);
insert into OrderDetails values(404,222,234765324,67,2,'Debit Card',1,0,123);

SELECT * FROM OrderDetails;
DROP TRIGGER trgTotalSale;
delete from totalSale;
drop table totalSale;
select * from totalSale;



--Instead of trigger===================
--Product Approval Table
Create table ProductApprovals(
	ProductName varchar(30),
	ApprovalStatus varchar(15)

)

--View of Products
Create view vw_Products
as
select Name,Detail from Products

--Test Data
insert into vw_Products values('t shirt 1','details about tshirt');
select * from vw_Products
select * from ProductApprovals


--TRIGGER
ALTER TRIGGER trg_vw_Products 
ON vw_Products
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO ProductApprovals 
	( 
        ProductName,
		ApprovalStatus
	)
    SELECT
        i.Name,
		'Pending'
    FROM
        inserted i
	 WHERE
        i.Name NOT IN (
            SELECT 
               Name
            FROM
                Products
        );
  
END