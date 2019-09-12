USE [MyECommerce]
GO
/****** Object:  StoredProcedure [dbo].[spRefund]    Script Date: 7/19/2019 7:30:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[spRefund](@OrderId int)
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE OrderDetails
	SET Refund = 1 
	WHERE OrderId = @OrderId  
    -- Insert statements for procedure here
	SELECT totalAmount from OrderDetails WHERE OrderId = @OrderId;
	
	
END
