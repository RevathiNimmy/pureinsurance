SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
EXEC DDLDropProcedure 'spu_CashDeposit_Branch_Add'
GO
 
CREATE  PROCEDURE spu_CashDeposit_Branch_Add    
 @CashDeposit_ID INT ,  
 @Branch_ID INT   
AS   
BEGIN  
 INSERT INTO   
  CashDeposit_Branch_Link(  
         CashDeposit_ID,  
         Branch_ID  
        )  
 VALUES (  
   @CashDeposit_ID,  
   @Branch_ID  
     )  
   
END  
