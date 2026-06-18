SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_IsDiscountApplied'
GO


CREATE PROCEDURE spu_IsDiscountApplied
@insurance_file_cnt INT,   
@is_discount_applied TINYINT  = NULL OUTPUT    
      
AS      
  
DECLARE @discount_applied INT    
      
DECLARE    Cur_Risk CURSOR FAST_FORWARD FOR    
        SELECT     R.is_discounted
        FROM       risk R
        INNER JOIN insurance_file_risk_link IFRL      
        ON         R.risk_cnt = IFRL.risk_cnt    
        WHERE      IFRL.insurance_file_cnt = @insurance_file_cnt      


OPEN Cur_Risk

FETCH NEXT FROM Cur_Risk    
INTO    
  @discount_applied  
  

WHILE @@FETCH_STATUS = 0
BEGIN
   IF @discount_applied = 0
      BEGIN
           SET @is_discount_applied = 0
      END
   ELSE IF @discount_applied = 1
      BEGIN
           SET @is_discount_applied = 1
           BREAK
      END


    FETCH NEXT FROM Cur_Risk
    INTO
         @discount_applied
END

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO


