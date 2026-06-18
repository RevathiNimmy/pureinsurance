EXECUTE DDLDropProcedure 'spu_GetRiskDetailsForBroking'
GO

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

/*  Author 		: Ram Chandrabose 
 	Created on 	: 11/09/2002
	Description	: This stored procedure is called from the claims moduele
	Referenced by	: bCLMRiskDetails.dll
*/
CREATE PROCEDURE spu_GetRiskDetailsForBroking
    @claim_id integer
AS
Begin
	DECLARE @insurance_file_cnt int
	DECLARE @risk_cnt int

	select @insurance_file_cnt = policy_id from claim Where claim_id = @claim_id
	select @risk_cnt = risk_cnt from insurance_file_risk_link where insurance_file_cnt =  @insurance_file_cnt

	SELECT  ifi.insurance_folder_cnt,
			ifi.insurance_file_cnt,
			ifi.product_id,
			rsk.risk_cnt,
			rsk.risk_type_id,
			rsk.gis_screen_id
	FROM  Insurance_file ifi,
		  Risk rsk
	where ifi.insurance_file_cnt = @insurance_file_cnt
	AND rsk.risk_cnt = @risk_cnt
END

GO