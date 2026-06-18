SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
EXEC DDLDropProcedure 'spu_CashDeposit_SinglePolicy_Upd'
GO
 
CREATE  PROCEDURE spu_CashDeposit_SinglePolicy_Upd  
	@CashDeposit_ID INT ,
	@Is_SinglePolicy TINYINT				
	
AS 
BEGIN
	UPDATE 
		CashDeposit 
	SET 		
		Is_SinglePolicy=@Is_SinglePolicy				
		
	WHERE 
		CashDeposit_ID=@CashDeposit_ID
					
END
 
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
