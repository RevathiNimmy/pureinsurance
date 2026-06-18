SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_sir_refresh_ifrl' 
GO

--- Description: Copies risk links between two versions of a insurance file. Must be called only for OOS
--- History:
--- 2012-09-07 - PM Created Procedure
CREATE PROCEDURE spu_sir_refresh_ifrl
    @nOldInsuranceFileCnt int,
    @nNewInsuranceFileCnt int,
    @bCopyDeletedRisks	 bit
AS
DECLARE @insurance_file_type INT
	
	DELETE FROM insurance_file_risk_link
		WHERE insurance_file_cnt = @nNewInsuranceFileCnt
		
	SELECT @insurance_file_type = insurance_file_type_id 
		FROM Insurance_File WHERE insurance_file_cnt = @nNewInsuranceFileCnt
	
	IF @insurance_file_type = 3 
		INSERT INTO insurance_file_risk_link (
						insurance_file_cnt,
						risk_cnt,
						status_flag)
		SELECT			@nNewInsuranceFileCnt,
						risk_cnt,
						'R'
		FROM	insurance_file_risk_link
		WHERE	insurance_file_cnt = @nOldInsuranceFileCnt
		AND     ((status_flag <> 'D' AND @bCopyDeletedRisks = 0) OR @bCopyDeletedRisks = 1)			    
	ELSE
		INSERT INTO insurance_file_risk_link (
						insurance_file_cnt,
						risk_cnt,
						status_flag)
		SELECT			@nNewInsuranceFileCnt,
						risk_cnt,
						CASE
							WHEN status_flag = 'C' THEN 'U'
							WHEN status_flag = 'R' THEN 'U'
							WHEN status_flag = 'D' AND @bCopyDeletedRisks = 1 THEN 'U'
							WHEN status_flag = 'D' AND @bCopyDeletedRisks = 0 THEN 'D'
							ELSE 'U'
						END
		FROM	insurance_file_risk_link
		WHERE	insurance_file_cnt = @nOldInsuranceFileCnt
		AND     ((status_flag <> 'D' AND @bCopyDeletedRisks = 0) OR @bCopyDeletedRisks = 1)			    

GO
