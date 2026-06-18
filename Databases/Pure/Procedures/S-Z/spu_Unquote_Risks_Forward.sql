SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Unquote_Risks_Forward'
GO

CREATE PROCEDURE spu_Unquote_Risks_Forward
	@nRiskCnt INT
AS  
/*
		ModifiedBy		Date		Description
		---------------------------------------------------------------------------------------------------------------------------------------------------------------
		GHarris			01/08/18	Added the additional where clause to only update where the risk status i not equal to 4
									Changed the status_flag to use an in statement
									REformatted proc and changed the where clause to use a enum

*/
BEGIN  

	DECLARE @nRiskFolderCnt INT
	DECLARE @enum_risk_status_unquoted INT = 4

	SELECT @nRiskFolderCnt = risk_folder_cnt 
	FROM risk 
	WHERE risk_cnt = @nRiskCnt

	-- Update all future risks to unquoted if quoted; use mta_insurance_file_link to ensure OOS MTA (NO OOS MTC or OOS MTR)
	UPDATE risk 
	SET risk_status_id = 4 
		FROM risk r
		INNER JOIN insurance_file_risk_link ifrl ON ifrl.risk_cnt = r.risk_cnt
		INNER JOIN mta_insurance_file_link mifl ON mifl.new_linked_insurance_file_cnt = ifrl.insurance_file_cnt 
			AND mifl.cancelled_linked_insurance_file_cnt IS NOT NULL
	WHERE r.risk_cnt > @nRiskCnt 
		AND risk_folder_cnt = @nRiskFolderCnt
		AND ifrl.status_flag in('C', 'D')
		AND r.risk_status_id <> @enum_risk_status_unquoted

END  
