SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SAM_Get_SavedPFPlan_On_Renewal'
GO

CREATE PROCEDURE spu_SAM_Get_SavedPFPlan_On_Renewal
	@insurance_file_cnt int
AS

BEGIN

    --UPDATE Insurance_File SET payment_method ='Instalments' WHERE insurance_file_cnt = @insurance_file_cnt

	SELECT	pfprem_finance_cnt,
			pfprem_finance_version,
			clientID,
			SchemeNo,
			SchemeName,
			SchemeVersion,
			TransType,
			PF.Insurance_File_Cnt,
			INF.Base_Insurance_File_Cnt,
			INF.payment_method,
			INF.insurance_file_status_id
	FROM PFPremiumFinance PF
		RIGHT OUTER JOIN Insurance_File INF ON INF.Insurance_File_Cnt = PF.insurance_file_cnt
	WHERE INF.insurance_file_cnt = @insurance_file_cnt

END
GO
