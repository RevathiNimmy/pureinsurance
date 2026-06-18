SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Get_Prior_Term_Scheme_Insurance_file_cnt'
GO
CREATE PROCEDURE spu_Get_Prior_Term_Scheme_Insurance_file_cnt
 @insurance_file_cnt int,
 @use_prior_term_scheme_at_ren bit = 0
AS
DECLARE @Prior_term_scheme_policy_version int,
@insurance_folder_cnt int

SELECT @insurance_folder_cnt = insurance_folder_cnt FROM insurance_file WHERE insurance_file_cnt = @insurance_file_cnt
IF @use_prior_term_scheme_at_ren =1
SELECT  TOP 1 @Prior_term_scheme_policy_version = insurance_file_cnt FROM insurance_file
					WHERE insurance_folder_cnt = @insurance_folder_cnt AND insurance_file_type_id = 2
								AND base_insurance_file_cnt IS NULL ORDER BY inception_date_tpi DESC
ELSE
SELECT TOP 1 @Prior_term_scheme_policy_version = pf.insurance_file_cnt FROM insurance_file inf
			INNER JOIN PFPremiumFinance pf ON inf.insurance_file_cnt = pf.Insurance_File_Cnt
			WHERE insurance_folder_cnt = @insurance_folder_cnt AND statusind IN ('040','140','900','990','999')
			ORDER BY pfprem_finance_cnt DESC,pfprem_finance_version DESC
SELECT @Prior_term_scheme_policy_version Prior_term_scheme_policy_version

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
