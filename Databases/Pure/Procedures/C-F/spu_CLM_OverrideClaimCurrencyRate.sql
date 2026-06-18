SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER OFF
GO
Execute DDLDropProcedure 'spu_CLM_OverrideClaimCurrencyRate'
GO

CREATE PROCEDURE spu_CLM_OverrideClaimCurrencyRate    
    @claim_id INT,    
    @OverriddenCurrencyRate FLOAT = NULL  
AS    

BEGIN    
 
  DECLARE @dtOpen_Claim_Date DATETIME    
     
  SELECT @dtOpen_Claim_Date = create_date FROM claim WHERE claim_id =    
   (SELECT claim_id FROM claim WHERE version_id = 1 AND claim_number = (SELECT claim_number FROM claim WHERE claim_id = @claim_id))    
  IF (@OverriddenCurrencyRate IS NULL OR @OverriddenCurrencyRate = 0)    
  
    UPDATE claim    
    SET currency_base_xrate = NULL,    
          currency_base_date = NULL    
    WHERE claim_id = @claim_id    
  ELSE  
    UPDATE claim    
    SET currency_base_xrate = @OverriddenCurrencyRate,    
          currency_base_date = @dtOpen_Claim_Date    
    WHERE claim_id = @claim_id    
  
  
END    

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON    
GO

