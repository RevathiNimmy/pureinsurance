/* Created by : Vidya Rangdale
Creation Date : 26/02/2014
Description   : This is used to update details of pftransaction_id table
Test Code     : Exec spu_PFTransaction_Id_UpdateForSingleInstalmentPlan
 */

SET QUOTED_IDENTIFIER OFF
GO
Execute DDLDropProcedure 'spu_PFTransaction_Id_UpdateForSingleInstalmentPlan'
GO
CREATE PROCEDURE spu_PFTransaction_Id_UpdateForSingleInstalmentPlan
   @nPFprem_finance_cnt INT,
   @nInsurance_file_cnt INT
AS
DECLARE @nPFprem_finance_cntOld int
DECLARE @nPFpremfinancecntCur Int  
DECLARE @nInsurancefilecntCur Int 
DECLARE @enumPFInstalments_StatusCollected INT = 3
DECLARE @enumInsfileTypeMTACancelled INT = 8

SELECT TOP 1 @nPFprem_finance_cntOld =pfprem_finance_cnt from pfpremiumfinance 
WHERE insurance_file_cnt=@nInsurance_file_cnt AND pfprem_finance_cnt<>@nPFprem_finance_cnt

DECLARE TransactionUpdate Cursor Fast_forward FOR 
 
SELECT pf.pfprem_finance_cnt,pf.insurance_file_cnt
FROM pftransaction_id pf
	INNER JOIN insurance_file ifi ON pf.insurance_file_cnt=ifi.insurance_file_cnt
WHERE pfprem_finance_cnt=@npfprem_finance_cntOld AND ifi.insurance_folder_cnt Not in 
(Select insurance_folder_cnt from insurance_file WHERE insurance_file_type_id=@enumInsfileTypeMTACancelled)
	GROUP BY pf.pfprem_finance_cnt,pf.insurance_file_cnt,ifi.insurance_folder_cnt 

OPEN TransactionUpdate

FETCH NEXT FROM TransactionUpdate INTO @npfpremfinancecntCur,@nInsurancefilecntCur
  
 WHILE @@FETCH_STATUS = 0 BEGIN  
  
    UPDATE pftransaction_id SET pfprem_finance_cnt=@npfprem_finance_cnt ,pfprem_finance_version=1 
    WHERE pfprem_finance_cnt=@nPFpremfinancecntCur AND insurance_file_cnt=@ninsurancefilecntCur
  
     FETCH NEXT FROM TransactionUpdate INTO @nPFpremfinancecntCur,@nInsurancefilecntCur
  
 END  
  
 CLOSE TransactionUpdate  
 DEALLOCATE TransactionUpdate  

GO
