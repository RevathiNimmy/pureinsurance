SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Get_Previous_Scheme_Version_From_InsuranceFile'
GO    
CREATE  PROCEDURE spu_Get_Previous_Scheme_Version_From_InsuranceFile    
    @insurance_file_cnt INT    
AS    
    
BEGIN    
    
DECLARE  @insurance_folder_cnt INT    
DECLARE  @previous_policy_version INT    
DECLARE  @previous_insurance_file_cnt INT    
    
-- Get insurance folder cnt    
SELECT @insurance_folder_cnt = ifol.insurance_folder_cnt    
FROM insurance_folder ifol JOIN insurance_file inf ON ifol.insurance_folder_cnt=inf.insurance_folder_cnt    
WHERE inf.insurance_file_cnt=@insurance_file_cnt    
    
-- Get previous policy version (file type limited)    
SELECT  @previous_policy_version = MAX(infi2.Policy_version),    
  @previous_insurance_file_cnt= infi2.insurance_file_cnt    
FROM    insurance_file AS infi2    
WHERE   infi2.insurance_folder_cnt = @insurance_folder_cnt    
AND  infi2.insurance_file_cnt<=@insurance_file_cnt    
-- Only include InsFileTypes POLICY, MTA, RENEWAL PERM & MTAREINS    
AND     infi2.insurance_file_type_id In (2, 3, 5, 9)  
GROUP BY infi2.insurance_file_cnt  
ORDER BY infi2.insurance_file_cnt ASC  
    
-- Get pfpremiumfinance details    
SELECT  pf.pfprem_finance_cnt,    
        pf.pfprem_finance_version,    
  pf.companyno,    
  pf.schemeno,    
  pf.schemeversion,    
  pf.schemename,    
  pf.productclass,    
  pf.transtype,
  pff.description    
FROM    insurance_file AS infi    
JOIN    pfpremiumfinance AS PF ON infi.insurance_file_cnt = pf.insurance_file_cnt  
JOIN pfrf ON pfrf.pfrf_id = PF.pfrf_id 
JOIN pffrequency pff ON pff.pffrequency_id = pfrf.pffrequency_id   
WHERE   infi.insurance_folder_cnt = @insurance_folder_cnt    
AND     infi.policy_version = @previous_policy_version    
--AND     pf.statusInd = '040'    
  
END    
  