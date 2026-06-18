SET QUOTED_IDENTIFIER ON 
GO

EXECUTE DDLDropProcedure 'spu_SAM_Update_InsFile_Risk_Link_Status'
GO

/*
	Created By		: Krishan Kumar Gorav
	Creation Date	: 05 Aug 2015
	Parameters		: @nInsuranceFileCnt - Insurance file cnt
					: @nRiskCnt -Risk key to update the status
	Description		: To update insurance file risk link status
	Test Code		: EXEC spu_SAM_Update_InsFile_Risk_Link_Status  123456,122
*/

CREATE PROCEDURE spu_SAM_Update_InsFile_Risk_Link_Status  
	@nInsuranceFileCnt INT,  
	@nRiskCnt INT
AS  
BEGIN  
	DECLARE @sStatusCode VARCHAR(10)

	SELECT @sStatusCode =Risk_Status.code FROM Risk LEFT JOIN Risk_Status ON Risk.risk_status_id=Risk_Status.risk_status_id WHERE risk_cnt=  @nRiskCnt
   	
	UPDATE Insurance_file_Risk_Link 
	SET status_flag = 'C'
	WHERE insurance_file_cnt = @nInsuranceFileCnt AND risk_cnt = @nRiskCnt AND @sStatusCode='QUOTED' AND status_flag='U'
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
