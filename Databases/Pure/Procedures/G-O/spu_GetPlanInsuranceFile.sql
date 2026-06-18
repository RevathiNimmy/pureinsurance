SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_GetPlanInsuranceFile'
GO


CREATE PROCEDURE spu_GetPlanInsuranceFile
	@InsuranceFileCnt int
 AS

SELECT TOP 1 pf.insurance_file_cnt
FROM	Insurance_File ifi INNER JOIN Insurance_File ifi2
ON	ifi.insurance_folder_cnt = ifi2.insurance_folder_cnt
	INNER JOIN pfPremiumFinance pf
ON	ifi2.insurance_file_cnt = pf.insurance_file_cnt
WHERE ifi.insurance_file_cnt = @InsuranceFileCnt
ORDER BY pf.pfprem_finance_version DESC
GO

