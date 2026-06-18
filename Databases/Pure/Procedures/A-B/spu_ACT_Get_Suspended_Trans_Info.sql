SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

Execute DDLDropProcedure 'spu_ACT_Get_Suspended_Trans_Info'
GO

-- Object:  Stored Procedure dbo.spu_ACT_Get_Suspended_Trans_Info   
-- Script Date: 11/4/2003 1:32:18 PM 

CREATE PROCEDURE spu_ACT_Get_Suspended_Trans_Info

    @TransDetailId INT

AS

DECLARE	@TransDetailAmount Numeric (19,4)
DECLARE	@AllocationDetailAmount Numeric (19,4)
DECLARE	@SuspendedAccountID int

SELECT 	@TransDetailAmount = Currency_Amount,  @SuspendedAccountID =  account_id
FROM 	TransDetail 
WHERE TransDetail_ID =  @TransDetailId


SELECT 	@AllocationDetailAmount = ISNULL(SUM(alloc_base_amount), 0) 
FROM 	AllocationDetail 
WHERE 	TransDetail_ID =  @TransDetailId

SELECT      @TransDetailAmount as TransDetailAmount,
                 @SuspendedAccountID as SuspendedAccountID,
                 @AllocationDetailAmount  as AllocationDetailAmount 


GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

