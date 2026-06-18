SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER OFF
GO
Execute DDLDropProcedure 'spu_CLM_GetCurrencyRatesToOverride'
GO

CREATE PROCEDURE spu_CLM_GetCurrencyRatesToOverride 
    @claim_id INT 
AS  
BEGIN
  
DECLARE @dtOpen_Claim_Date DATETIME,    
   @dtCurrentDate DATETIME,    
   @CurrencyId INT,    
   @SourceId INT,    
   @NewCurrencyRate FLOAT ,    
   @OldCurrencyRate FLOAT,  
   @user_id INT,    
   @can_change_exchange_date INT,  
   @can_change_exchange_rate INT,  
   @product_id INT,  
   @allow_loss_currency_change INT  
  
    SET @NewCurrencyRate = -1    
    SET @OldCurrencyRate = -1    
 SET @dtOpen_Claim_Date = NULL    
 SET @dtCurrentDate = GetDate()    
    
 SELECT @user_id = created_by_id FROM claim WHERE claim_id = @claim_id   
   
 SELECT @can_change_exchange_date = can_change_exchange_date, @can_change_exchange_rate = can_change_exchange_rate  
 FROM user_authorities ua  
 JOIN PMUser pmu ON pmu.user_id = ua.user_id  
 WHERE pmu.user_id = @user_id  
  
 SET @can_change_exchange_date = ISNULL(@can_change_exchange_date, 0 )  
 SET @can_change_exchange_rate = ISNULL(@can_change_exchange_date, 0 )  
  
IF @can_change_exchange_date = 1 OR @can_change_exchange_rate = 1  
BEGIN   
  SELECT @CurrencyId = currency_id FROM claim where claim_id = @claim_id    
  SELECT @SourceId = source_id, @product_id = product_id FROM insurance_file   
  WHERE insurance_file_cnt = (SELECT policy_id FROM  claim WHERE claim_id = @claim_id)    
  
  SELECT @allow_loss_currency_change = allow_loss_currency_change FROM product where product_id = @product_id  
     SET @allow_loss_currency_change = ISNULL(@allow_loss_currency_change, 0)   
  
  IF @allow_loss_currency_change = 1   
     BEGIN     
     
         SELECT @dtOpen_Claim_Date = create_date FROM claim WHERE claim_id =    
   (SELECT claim_id FROM claim WHERE version_id = 1 AND claim_number = (SELECT claim_number FROM claim WHERE claim_id = @claim_id))    
   EXEC spu_ACT_Get_Currency_Rate @currency_id=@CurrencyId,@company_id=@SourceId,@effective_date=@dtCurrentDate,@rate=@NewCurrencyRate OUTPUT    
   EXEC spu_ACT_Get_Currency_Rate @currency_id=@CurrencyId,@company_id=@SourceId,@effective_date=@dtOpen_Claim_Date,@rate=@OldCurrencyRate OUTPUT    
   IF @NewCurrencyRate IS NULL OR @OldCurrencyRate IS NULL OR @OldCurrencyRate = -1 OR @NewCurrencyRate = 0 OR @OldCurrencyRate = 0    
   BEGIN    
    SET @NewCurrencyRate = -1    
    SET @OldCurrencyRate = -1    
   END   
     END    
END   
   
    SELECT @NewCurrencyRate As NewCurrencyRate, @OldCurrencyRate As OldCurrencyRate   

END

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON    
GO
