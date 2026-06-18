SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
EXEC DDLDropProcedure 'spu_CashDeposit_Branch_Del'
GO
  
CREATE  PROCEDURE spu_CashDeposit_Branch_Del    
 @CashDeposit_ID INT  
AS   
BEGIN  
 DELETE   
  CashDeposit_Branch_Link  
 WHERE   
  CashDeposit_ID=@CashDeposit_ID  
   
END  
