SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Import_Update_Instalment_Transaction_Code'
GO

-- ADO #39336: Modified to support Claim Recovery transaction type mapping
-- When plan transtype is 'SR' or 'TPR' (Claim Recovery), map:
--   INC -> ICC (Instalment Claim Credit)
--   IND -> ICD (Instalment Claim Debit)
CREATE PROCEDURE spu_ACT_Import_Update_Instalment_Transaction_Code

@pfprem_finance_cnt int, 
@pfprem_finance_version int, 
@pfinstalments_transaction_code varchar(10)

AS

DECLARE @resolved_code varchar(10)
DECLARE @plan_transtype varchar(10)

-- Check if this is a claim recovery plan
SELECT @plan_transtype = LTRIM(RTRIM(transtype))
FROM PFPremiumFinance
WHERE pfprem_finance_cnt = @pfprem_finance_cnt
  AND pfprem_finance_version = @pfprem_finance_version

-- Map INC/IND to ICC/ICD for claim recovery plans
IF @plan_transtype IN ('SR', 'TPR')
BEGIN
    SET @resolved_code = CASE @pfinstalments_transaction_code
        WHEN 'INC' THEN 'ICC'
        WHEN 'IND' THEN 'ICD'
        ELSE @pfinstalments_transaction_code
    END
END
ELSE
BEGIN
    SET @resolved_code = @pfinstalments_transaction_code
END

DECLARE @transaction_code_id INT

SELECT @transaction_code_id = pfinstalments_transaction_id
FROM pfinstalments_transaction
WHERE code = @resolved_code

IF @transaction_code_id IS NULL
    RETURN

UPDATE pfinstalments 
SET transactioncode = @transaction_code_id
WHERE pfinstalments_id in (

	SELECT MIN(pfinstalments_id) pfinstalments_id
	FROM pfinstalments 
	WHERE status in (SELECT pfinstalments_status_id 
			 FROM pfinstalments_status 
			 WHERE code in ('U', 'R', 'H'))
	AND pfprem_finance_cnt = @pfprem_finance_cnt
	AND pfprem_finance_version = @pfprem_finance_version
	AND instalmentNumber <> 0 )



GO
