SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_PFPremiumFinance_inhouse'
GO
CREATE PROCEDURE spe_PFPremiumFinance_inhouse
    @FinancePlanCnt int,
    @FinancePlanVersion int,
    @IsInHouse tinyint OUTPUT,
    @PFPaymentMethod_cnt INT OUTPUT
AS

    SELECT
        @IsInHouse = IsNull(IsInHouse, 0),
        @PFPaymentMethod_cnt = S.PaymentMethod_cnt

    FROM
        pfscheme S

    INNER JOIN
        pfpremiumfinance PF

    ON
        S.companyno = PF.companyno
    AND
        S.schemeno = PF.schemeno
    AND
        S.schemeversion = PF.schemeversion

    WHERE
        PF.pfprem_finance_cnt = @FinancePlanCnt
    AND
        PF.pfprem_finance_version = @FinancePlanVersion

    SELECT @IsInHouse = IsNull(@IsInHouse, 0)
GO

