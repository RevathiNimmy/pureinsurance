SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

DDLDropProcedure 'spu_CLR_RecoveryInstalmentPlan_Validate'
GO

-- ADO #39455: Instalment for Claim Recovery - Scheme Configuration
-- Check if an active instalment plan already exists for a CLR recovery transaction
-- Returns 1 if active plan exists (block creation), 0 if no active plan
CREATE PROCEDURE spu_CLR_RecoveryInstalmentPlan_Validate
    @ClrTransactionId INT,
    @HasActivePlan TINYINT OUTPUT
AS
BEGIN
    SET @HasActivePlan = 0

    IF EXISTS (
        SELECT 1
        FROM PFPremiumFinance pf
        WHERE pf.claim_recovery_transaction_id = @ClrTransactionId
          AND pf.StatusInd NOT IN ('900', '999')
    )
    BEGIN
        SET @HasActivePlan = 1
    END
END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
