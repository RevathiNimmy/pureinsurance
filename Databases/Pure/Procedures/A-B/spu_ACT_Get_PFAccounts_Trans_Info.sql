SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_ACT_Get_PFAccounts_Trans_Info'
GO

-- Object:  Stored Procedure dbo.spu_ACT_Get_PFAccounts_Trans_Info   
-- Script Date: 10/21/2003 8:24:11 AM ******/

CREATE PROCEDURE spu_ACT_Get_PFAccounts_Trans_Info

    @TransDetailId INT

AS

DECLARE	@TransDetailAmount Numeric (19,4)
DECLARE	@AllocationDetailAmount Numeric (19,4)
DECLARE	@AccountID int

SELECT 	@TransDetailAmount = Currency_Amount,  @AccountID =  account_id
FROM 	TransDetail 
WHERE   TransDetail_ID =  @TransDetailId


SELECT 	@AllocationDetailAmount = ISNULL(SUM(alloc_base_amount), 0) 
FROM 	AllocationDetail 
WHERE 	TransDetail_ID =  @TransDetailId

SELECT  @TransDetailAmount as TransDetailAmount,
        @AccountID as AccountID,
        @AllocationDetailAmount  as AllocationDetailAmount 


GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

