EXECUTE DDLDropProcedure 'spu_PFCheckSchemeType'
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE spu_PFCheckSchemeType
    @FinancePlanCnt int,
    @FinancePlanVersion int,
    @PFScheme_Type_code varchar(20) OUTPUT,
    @PFPaymentMethod_cnt INT OUTPUT
AS
    SELECT
        @PFScheme_Type_code = IsNull(st.code, ''),
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
    INNER JOIN
        pfScheme_type st
    ON
        s.pfScheme_type_id = st.pfScheme_type_id
   WHERE
        PF.pfprem_finance_cnt = @FinancePlanCnt
    AND
        PF.pfprem_finance_version = @FinancePlanVersion
 

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
