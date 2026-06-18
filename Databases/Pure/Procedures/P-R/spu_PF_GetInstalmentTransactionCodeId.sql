SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

DDLDropProcedure 'spu_PF_GetInstalmentTransactionCodeId'
GO

-- ADO #39336: Instalment for Claim Recovery - Transaction Code Resolution
-- Resolves the correct pfinstalments_transaction_id based on plan type.
-- For claim recovery plans (transtype = 'SR'/'TPR'), returns ICC/ICD IDs.
-- For standard PF plans, returns the standard INC/IND IDs.
--
-- @RequestedCode: The standard code being requested ('INC' or 'IND')
-- Returns: The correct pfinstalments_transaction_id to use
CREATE PROCEDURE spu_PF_GetInstalmentTransactionCodeId
    @pfprem_finance_cnt INT,
    @pfprem_finance_version INT,
    @RequestedCode VARCHAR(10),
    @TransactionCodeId INT OUTPUT
AS
BEGIN
    DECLARE @plan_transtype VARCHAR(10)

    SELECT @plan_transtype = LTRIM(RTRIM(transtype))
    FROM PFPremiumFinance
    WHERE pfprem_finance_cnt = @pfprem_finance_cnt
      AND pfprem_finance_version = @pfprem_finance_version

    DECLARE @resolved_code VARCHAR(10)

    IF @plan_transtype IN ('SR', 'TPR')
    BEGIN
        SET @resolved_code = CASE @RequestedCode
            WHEN 'INC' THEN 'ICC'
            WHEN 'IND' THEN 'ICD'
            ELSE @RequestedCode
        END
    END
    ELSE
    BEGIN
        SET @resolved_code = @RequestedCode
    END

    SELECT @TransactionCodeId = pfinstalments_transaction_id
    FROM pfinstalments_transaction
    WHERE code = @resolved_code

    IF @TransactionCodeId IS NULL
        SET @TransactionCodeId = 0
END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
