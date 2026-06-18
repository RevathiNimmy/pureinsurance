EXECUTE DDLDropProcedure 'spu_PF_GetFinancePlanDetails'
GO
CREATE PROCEDURE spu_PF_GetFinancePlanDetails
    @pfinstalments_id INT

AS

/*
    This stored procedure return finance plan count and their version
*/

SELECT
   P.pfprem_finance_cnt,P.pfprem_finance_version
FROM
    PFInstalments I
INNER JOIN
    PFPremiumFinance P ON P.pfprem_finance_cnt = I.pfprem_finance_cnt AND P.pfprem_finance_version = I.pfprem_finance_version
WHERE
    I.pfinstalments_id=@pfinstalments_id
Go

