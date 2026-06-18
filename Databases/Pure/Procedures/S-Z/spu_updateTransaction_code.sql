
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_updateTransaction_code'
GO

-- ADO #39336: Modified to support Claim Recovery transaction type mapping
-- For claim recovery plans (transtype = 'SR' or 'TPR'), use ICC transaction code
-- instead of hardcoded INC (ID=3)
CREATE PROCEDURE spu_updateTransaction_code  
 @pfprem_finance_cnt  INT,  
 @pfprem_finance_version  INT
  
AS

DECLARE @transaction_code_id INT
DECLARE @plan_transtype varchar(10)

-- Check if this is a claim recovery plan
SELECT @plan_transtype = LTRIM(RTRIM(transtype))
FROM PFPremiumFinance
WHERE pfprem_finance_cnt = @pfprem_finance_cnt
  AND pfprem_finance_version = @pfprem_finance_version

IF @plan_transtype IN ('SR', 'TPR')
BEGIN
    -- Use ICC (Instalment Claim Credit) for claim recovery
    SELECT @transaction_code_id = pfinstalments_transaction_id
    FROM pfinstalments_transaction
    WHERE code = 'ICC'

    IF @transaction_code_id IS NULL
        RETURN
END
ELSE
BEGIN
    -- Use INC (ID=3) for standard Premium Finance (backward compatible)
    SET @transaction_code_id = 3
END

UPDATE PFInstalments SET TransactionCode = @transaction_code_id   
WHERE  pfprem_finance_cnt = @pfprem_finance_cnt  
AND pfprem_finance_version = @pfprem_finance_version  
AND InstalmentNumber = 1
GO
