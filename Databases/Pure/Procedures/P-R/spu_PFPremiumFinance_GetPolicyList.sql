SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_PFPremiumFinance_GetPolicyList'
GO

CREATE PROCEDURE spu_PFPremiumFinance_GetPolicyList    
    @pfprem_finance_cnt Int,    
    @pfprem_finance_version Int,    
    @History Tinyint    
AS    
IF @History=1    
BEGIN    
    
SELECT ifl.insurance_file_cnt,    
   ifl.insurance_ref,    
   ifl.insured_name,    
  (SELECT SUM(ISNULL(ifi1.this_premium,0)) FROM insurance_file ifi1 WHERE ifi1.insurance_folder_cnt = ifl.insurance_folder_cnt AND    
 ifi1.insurance_file_type_id IN(2,5) AND ifi1.insurance_file_cnt IN (SELECT pft1.insurance_file_cnt FROM PFtransaction_id pft1 WHERE pft1.pfprem_finance_cnt<=@pfprem_finance_cnt)),    
   ifd.inception_date,   
   ifl.insured_cnt,    
   ifl.insurance_folder_cnt,    
   (SELECT SUM(ISNULL(ifi.this_premium,0)+ISNULL(ifi.tax_amount,0))  from insurance_file ifi where ifi.insurance_folder_cnt = ifl.insurance_folder_cnt    
    AND ifi.insurance_file_type_id = 8) As CancelAmount,    
   ifl.tax_amount    
FROM insurance_file ifl    
LEFT JOIN PFTransaction_id pft ON pft.insurance_file_cnt=ifl.insurance_file_cnt    
INNER JOIN PFPremiumFinance pfpm ON pfpm.insurance_file_cnt=pft.insurance_file_cnt    
LEFT JOIN insurance_folder ifd ON ifd.insurance_folder_cnt=ifl.insurance_folder_cnt    
WHERE pft.pfprem_finance_cnt=@pfprem_finance_cnt    
AND pft.pfprem_finance_version<=@pfprem_finance_version   
AND ifl.insurance_file_type_id <> 8    
AND ifl.insurance_file_cnt=(SELECT MAX(insurance_file_cnt) FROM insurance_file WHERE ifl.insurance_folder_cnt = insurance_file.insurance_folder_cnt AND insurance_file.insurance_file_type_id <> 8 AND insurance_file.insurance_file_cnt IN 
(SELECT pft4.insurance_file_cnt FROM PFtransaction_id pft4 WHERE pft4.pfprem_finance_cnt<=@pfprem_finance_cnt AND pft4.pfprem_finance_version<=@pfprem_finance_version))    
--AND ifl.insurance_folder_cnt not in    
--(Select insurance_folder_cnt from insurance_file where insurance_file.insurance_folder_cnt = ifl.insurance_folder_cnt    
 --and insurance_file.insurance_file_type_id = 8 And pfpm.StatusInd <> 999 )    
GROUP BY ifl.insurance_file_cnt,    
   ifl.insurance_ref,    
   ifl.insured_name,    
   ifl.this_premium,    
   ifd.inception_date,    
   ifl.insured_cnt,    
   ifl.insurance_folder_cnt,    
   ifl.tax_amount    
End    
IF @History=0    
BEGIN    
SELECT ifl.insurance_file_cnt,    
   ifl.insurance_ref,    
   ifl.insured_name,    
   ifl.this_premium ,    
   ifd.inception_date,    
   ifl.insured_cnt,    
   ifl.insurance_folder_cnt,    
   (SELECT SUM(ISNULL(ifi.this_premium,0)+ISNULL(ifi.tax_amount,0))  from insurance_file ifi where ifi.insurance_folder_cnt = ifl.insurance_folder_cnt    
    AND ifi.insurance_file_type_id = 8) As CancelAmount,    
   ifl.tax_amount,  
   ifl.cover_start_date     
FROM insurance_file ifl    
--LEFT JOIN PFTransaction_id pft ON pft.insurance_file_cnt=ifl.insurance_file_cnt  
INNER JOIN PFPremiumFinance pfpm ON pfpm.insurance_file_cnt=ifl.insurance_file_cnt  
LEFT JOIN insurance_folder ifd ON ifd.insurance_folder_cnt=ifl.insurance_folder_cnt  
WHERE pfpm.pfprem_finance_cnt=@pfprem_finance_cnt    
GROUP BY ifl.insurance_file_cnt,    
   ifl.insurance_ref,    
   ifl.insured_name,    
   ifl.this_premium,    
   ifd.inception_date,    
   ifl.insured_cnt,    
   ifl.insurance_folder_cnt,    
   ifl.tax_amount,  
   ifl.cover_start_date     
END 
Go