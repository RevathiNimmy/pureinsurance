EXECUTE DDLDropProcedure 'spu_ACT_GetDetailsForCommissionPost'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROC spu_ACT_GetDetailsForCommissionPost
    @PlanTransID int,
    @PremiumFinanceCnt int,
    @PremiumFinanceVersion int,
    @InstalmentID int

AS BEGIN

    DECLARE @CommissionAccountID int
    DECLARE @CommissionSuspenseTransID int
    DECLARE @LastInstalment tinyint
    DECLARE @InstalmentAmount numeric(19, 4)
    DECLARE @PFAmount numeric(19, 4)
    DECLARE @FeeAccountID int

    SELECT
        @CommissionAccountID = s.commission_suspense_account_id,
        @PFAmount = p.TotalCost,
        @CommissionSuspenseTransID = p.Commission_TransDetail_ID,
        @FeeAccountID = s.interest_account_id
    FROM pfPremiumFinance p, pfScheme s
    WHERE
        pfPrem_Finance_cnt = @PremiumFinanceCnt
    AND pfPrem_Finance_version = @PremiumFinanceVersion
    AND p.CompanyNo = s.CompanyNo
    AND p.SchemeNo = s.SchemeNo
    AND p.SchemeVersion = s.SchemeVersion

    SELECT
        @LastInstalment = CASE TransactionCode
                          WHEN 6 THEN 1
                          ELSE 0
                          END,
        @InstalmentAmount = Amount
    FROM pfInstalments
    WHERE pfInstalments_ID = @InstalmentID

    SELECT
        @CommissionSuspenseTransID AS CommissionSuspenseTransID,
        @InstalmentAmount/@PFAmount AS Percentage,
        @LastInstalment AS IsLastInstalment,
        @FeeAccountID AS FeeAccountID
END
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
