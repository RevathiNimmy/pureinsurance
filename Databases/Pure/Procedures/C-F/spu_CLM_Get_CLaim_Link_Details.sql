SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_CLaim_Link_Details'
GO

CREATE PROCEDURE spu_CLM_Get_CLaim_Link_Details

@claim_id int

AS

DECLARE @OptionValue VARCHAR(3)
SELECT @OptionValue=value from hidden_options where option_number=1

IF @OptionValue='U'
BEGIN
	SELECT  ifi.insurance_folder_cnt,
		ifi.insurance_file_cnt,
		ifi.product_id,
		rsk.risk_cnt,
		rsk.risk_type_id,
		rsk.gis_screen_id
	FROM    Claim clm,
		Insurance_file ifi,
		insurance_file_risk_link ifrl,
		Risk rsk
	WHERE clm.claim_id = @claim_id
	AND clm.policy_id = ifi.insurance_file_cnt
	AND ifi.insurance_file_cnt=ifrl.insurance_file_cnt
	AND ifrl.risk_cnt= rsk.risk_cnt
	AND clm.risk_type_id=rsk.risk_cnt
END
ELSE
BEGIN

	SELECT  ifi.insurance_folder_cnt,
		ifi.insurance_file_cnt,
		ifi.product_id,
		rsk.risk_cnt,
		rsk.risk_type_id,
		rsk.gis_screen_id
	FROM    Claim clm,
		Insurance_file ifi,
		insurance_file_risk_link ifrl,
		Risk rsk
	WHERE clm.claim_id = @claim_id
	AND clm.policy_id = ifi.insurance_file_cnt
	AND ifi.insurance_file_cnt=ifrl.insurance_file_cnt
	AND ifrl.risk_cnt= rsk.risk_cnt

END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
