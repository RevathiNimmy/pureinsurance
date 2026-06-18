
/* Created by : Vidya Rangdale
Creation Date : 26/02/2014
Description   : This is used to select details from PFPremiumFinance table
Test Code     : Exec spu_PFPremiumFinance_Sel_Single_NextDate  
 */

SET QUOTED_IDENTIFIER ON
GO
Execute DDLDropProcedure 'spu_PFPremiumFinance_Sel_Single_NextDate'
GO

CREATE PROCEDURE spu_PFPremiumFinance_Sel_Single_NextDate  
	@nPFprem_finance_cnt int,  
	@nPFprem_finance_version int  
AS  
DECLARE @enumPFInstalments_StatusCollected INT = 3

SELECT  CASE PF.NoOfInstallments
        WHEN 1 THEN PF.next_instalment_date
        ELSE
            (SELECT MIN(DUEDATE)  FROM pfinstalments i
            WHERE i.pfprem_finance_cnt = @nPFprem_finance_cnt  
            AND i.pfprem_finance_version = @nPFprem_finance_version
            AND i.Status <> @enumPFInstalments_StatusCollected AND i.InstalmentNumber <> 0 AND i.InstalmentNumber>1) 
        END
    FROM PFPremiumFinance PF
    WHERE PF.pfprem_finance_cnt = @nPFprem_finance_cnt
    AND PF.pfprem_finance_version = @nPFprem_finance_version

GO

