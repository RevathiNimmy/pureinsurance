SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Insurer_Business_Summary'
GO
------------------------------------------------
-- Created by : Elaine Knott
-- Date : 10/07/2002
-- Description : Summary of Insurer Business for Effective Date Range
------------------------------------------------
CREATE PROCEDURE spu_Report_Insurer_Business_Summary
    @branch_id int,
    @start_date datetime,
    @end_date datetime
AS

DECLARE @iBranchID int

SELECT @iBranchID = ISNULL(@branch_id, 0)

SELECT A.short_code Account_Code,
    A.account_name Account_Name,
    ISNULL( ( SELECT SUM(TD.amount) - SUM(TD.ref_amount)
            FROM Transdetail TD

            WHERE TD.spare = 'GROSS'
            AND TD.account_id = A.account_id
            AND TD.accounting_date >= @start_date
            AND TD.accounting_date <= @end_date
            AND (@iBranchID = 0
                    OR
                    (@iBranchID <> 0
                    AND
                    TD.company_id = @iBranchID
                    )
                )
        )
, 0) Gross_Premium,
    ISNULL( ( SELECT SUM(TD.amount)
            FROM Transdetail TD

            WHERE (TD.spare = 'COMM' OR TD.spare = 'COMM ADJ')
            AND TD.accounting_date >= @start_date
            AND TD.accounting_date <= @end_date
            AND (@iBranchID = 0
                    OR
                    (@iBranchID <> 0
                    AND
                    TD.company_id = @iBranchID
                    )
                )
            AND TD.account_id = A.account_id

        )
, 0) Commission
INTO #TempTable
FROM Account A
GROUP BY A.account_id, A.short_Code, A.account_name

DELETE FROM #TempTable
WHERE Gross_Premium = 0 and Commission = 0

UPDATE #TempTable

SET gross_premium = gross_premium * -1

SELECT account_code, account_name, gross_premium, commission FROM #TempTable

DROP TABLE #TempTable
GO

