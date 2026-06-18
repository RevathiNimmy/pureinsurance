SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_get_current_PF_for_insurance_file'
GO

CREATE PROCEDURE spu_get_current_PF_for_insurance_file
    @insurance_file_cnt int
AS
BEGIN
SELECT TOP 1
	pf.pfPrem_finance_cnt, 
	pf.pfPrem_finance_version, 
	insF.insurance_Ref PolicyNumber 
FROM 
	PFPremiumFinance pf 
INNER JOIN Insurance_File insF 
	ON pf.insurance_file_cnt = insF.insurance_file_cnt 
WHERE 
	insF.insurance_file_cnt = @insurance_file_cnt
	AND pf.StatusInd in ('040', '140', '900', '990', '999', '010', '011')
ORDER BY
	pf.StatusInd

END

GO