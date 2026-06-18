SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
EXEC DDLDropProcedure 'spu_CashDeposit_Product_Add'
GO
CREATE  PROCEDURE spu_CashDeposit_Product_Add   
 @CashDeposit_ID INT ,  
 @Product_ID INT   
AS   
BEGIN  
 INSERT INTO   
  CashDeposit_Product_Link(  
         CashDeposit_ID,  
         Product_ID  
        )  
 VALUES (  
   @CashDeposit_ID,  
   @Product_ID  
     )  
   
END  
