
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_ACT_CurrencyBaseRateByAccount'
GO

CREATE PROCEDURE spu_ACT_CurrencyBaseRateByAccount
@account_id INT,
@company_id INT,
@account_base_date	Date ,
@account_base_xrate  MONEY=NULL OUTPUT

AS

Declare @CurrencyId int

   SELECT @CurrencyId =currency_id from Account where account_id=@account_id
   
   SELECT @Account_Base_XRate=1
   
   SELECT
   @account_base_xrate=ISNULL(CR.rate_against_base,1)
   FROM   CurrencyRate CR  
   JOIN   Currency C  
    ON C.currency_id = CR.currency_id  
   WHERE  CR.currency_id = @CurrencyId  
       AND CR.company_id = @company_id  
       AND CR.effective_from IN (SELECT MAX(effective_from)  
            FROM   CurrencyRate  
            WHERE  effective_from <= @account_base_date
             AND currency_id = CR.currency_id  
             AND company_id = CR.company_id)