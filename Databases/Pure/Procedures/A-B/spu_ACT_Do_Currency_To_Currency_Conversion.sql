SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Do_Currency_To_Currency_Conversion'
GO


CREATE PROCEDURE spu_ACT_Do_Currency_To_Currency_Conversion  
  
@currency_id_from int,  
@currency_amount_from money,  
@company_id int,  
@currency_id_to int,  
@effective_date datetime=NULL,  
@currency_amount_to money OUTPUT,  
@base_amount_unrounded money = 0 OUTPUT  
  
AS  
  
BEGIN  
  
 DECLARE @base_amount_unrounded_local money  
 DECLARE @base_currency_id_local money  
 DECLARE @return_status_local int  
 DECLARE @CurrencyTo_to_base_rate money  
  
 SELECT @effective_date = ISNULL(@effective_date,GETDATE())  
  
 -- convert currency1 amount to base  
 -- NB: mode is set to 1 because otherwise it defaults to all  
 -- and does a whole load of stuff we dont need here.  
 EXEC spu_ACT_Do_Currency_Conversion @company_id = @company_id,  
         @currency_id = @currency_id_from,  
         @currency_amount_unrounded = @currency_amount_from,  
         @currency_base_date = @effective_date,  
         @mode ='1',  
         @base_amount_unrounded =@base_amount_unrounded_local OUTPUT,  
         @base_currency_id = @base_currency_id_local OUTPUT,  
         @return_status = @return_status_local  
  
 -- get the currency rate from base to new currency  
 EXEC spu_ACT_Get_Currency_Rate  @currency_id = @currency_id_to,  
     @company_id =@company_id,  
     @effective_date = @effective_date,  
     @rate = @CurrencyTo_to_base_rate OUTPUT  
  
 -- determine the currency to rate = (base amount / currency_rate)  
 SET @currency_amount_to = (@base_amount_unrounded_local / @CurrencyTo_to_base_rate)  
 SET @base_amount_unrounded = @base_amount_unrounded_local  
  
END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
