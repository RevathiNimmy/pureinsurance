SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO
DDLDROPPROCEDURE 'spu_SAM_SelAll_BankAccount'
GO
CREATE PROCEDURE spu_SAM_SelAll_BankAccount
    @company_id INT,
    @bankaccount_id INT
AS

DECLARE @sSQL VARCHAR(2000)

IF @company_id = 0
SELECT @company_id = NULL

IF @bankaccount_id = 0
SELECT @bankaccount_id = NULL

/*select the bank accounts*/
 SELECT @sSQL='	SELECT bankaccount_id,
       b.code,
       bank_account_no,
       b.description,
       b.effective_date,
       b.is_deleted,
       b.currency_id,
       c.code CurrencyCode,
       b.bank_account_name		
		FROM   BankAccount b Join Currency c
		on b.currency_id=c.currency_id
		WHERE ' 

IF @company_id IS NOT NULL AND @bankaccount_id IS NOT NULL
BEGIN
SELECT @sSQL=@sSQL + '(
			company_id = ' + CONVERT(VARCHAR,@company_id) 
			 + ' OR ' + 
			CONVERT(VARCHAR,@company_id) + ' IS NULL
			)
			AND
			(
			        bankaccount_id = ' + CONVERT(VARCHAR,@bankaccount_id)
			         + ' OR ' + 
			        CONVERT(VARCHAR,@bankaccount_id) +' IS NULL 
			)
			AND b.is_deleted <> 1 AND bank_id is not NULL '
END
ELSE IF @bankaccount_id IS NULL
BEGIN
SELECT @sSQL=@sSQL + 'bankaccount_id IN (select bankaccount_id from bankaccount_source where source_id = ' + CONVERT(VARCHAR,@company_id)+  ' )'
SELECT @sSQL=@sSQL + ' AND b.is_deleted <> 1 AND bank_id is not NULL'  
END

EXECUTE(@sSQL)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

