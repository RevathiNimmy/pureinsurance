SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_Restore_MTA_Link'
GO

CREATE PROCEDURE spu_SIR_Restore_MTA_Link
	@nBaseInsuranceFileCnt INT
AS  
BEGIN  
	Declare @nCancelledFileCnt INT

	SELECT @nCancelledFileCnt = 0
	SELECT @nCancelledFileCnt = mifl_Old.insurance_file_cnt FROM mta_insurance_file_link mifl_New
		Inner Join mta_insurance_file_link mifl_Old 
			ON mifl_Old.cancelled_linked_insurance_file_cnt = mifl_New.original_linked_insurance_file_cnt 
					And mifl_New.original_insurance_file_status_id = 1 And mifl_Old.processed_ind = 1
		WHERE mifl_new.insurance_file_cnt = @nBaseInsuranceFileCnt

	-- OOS reinstatement
	IF @nCancelledFileCnt > 0
	BEGIN
		Update mta_insurance_file_link SET processed_ind = 0 WHERE insurance_file_cnt = @nCancelledFileCnt
	END
	
	DELETE FROM mta_insurance_file_link WHERE insurance_file_cnt = @nBaseInsuranceFileCnt
END  
