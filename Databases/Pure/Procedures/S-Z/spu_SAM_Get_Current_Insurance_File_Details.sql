SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Get_Current_Insurance_File_Details'
GO

CREATE PROCEDURE spu_SAM_Get_Current_Insurance_File_Details
@insurance_file_cnt int 
AS 
BEGIN
SELECT DISTINCT
	insf2.insurance_file_cnt AS original_insurance_file_cnt, 
	pf.pfprem_finance_cnt, 
	pf.pfprem_finance_version,
	ifs.description as insurance_file_status_description   , 		
	pf.party_bank_id  	
FROM 
	insurance_file insf
INNER JOIN insurance_file_risk_link AS ifrl 
		ON insf.insurance_file_cnt = ifrl.insurance_file_cnt
INNER JOIN insurance_file_risk_link AS ifrl2 
		ON ifrl.original_risk_cnt = ifrl2.risk_cnt
INNER JOIN insurance_file AS insf2 
		ON ifrl2.insurance_file_cnt = insf2.insurance_file_cnt
LEFT OUTER JOIN pfpremiumfinance AS pf
		ON pf.insurance_file_cnt = insf2.insurance_file_cnt
LEFT OUTER JOIN Insurance_File_Status AS ifs    
        ON ifs.insurance_file_status_id = insf2.insurance_file_status_id  
WHERE 
	insf.insurance_file_cnt = @insurance_file_cnt                
END
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

