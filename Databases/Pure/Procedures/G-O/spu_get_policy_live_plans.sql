SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_get_policy_live_plans'
GO

CREATE PROCEDURE spu_get_policy_live_plans
@Insurance_file_cnt INT
AS
SELECT PFPrem_Finance_Cnt,
        PFPrem_Finance_Version,
        StatusInd
        FROM PFPremiumFinance PF
        INNER JOIN insurance_file ifl ON ifl.insurance_file_cnt=PF.insurance_file_cnt
		INNER JOIN insurance_file i ON i.insurance_folder_cnt=ifl.insurance_folder_cnt
WHERE
    i.insurance_file_cnt=@Insurance_file_cnt AND pf.StatusInd IN ('040','140')

GO

