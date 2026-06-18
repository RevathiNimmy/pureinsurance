SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Get_original_risk_status'
GO

CREATE PROCEDURE spu_SIR_Get_original_risk_status
	@nOriginal_insurance_file_cnt int,  
	@nRisk_cnt int
AS  
  
BEGIN  

DECLARE @nOriginal_iFileCnt INT	
DECLARE @bIs_risk_edited TINYINT
DECLARE @sStatus VARCHAR(1)
DECLARE @nRisk_folder_cnt INT	

SELECT @bIs_risk_edited = 0

	SELECT @nRisk_folder_cnt = risk_folder_cnt
			FROM risk WHERE risk_cnt = @nRisk_cnt
		
	SELECT @bIs_risk_edited = ISNULL(is_risk_edited, 0), @sStatus = status_flag
			FROM insurance_file_risk_link ifrl
		WHERE insurance_file_cnt = @nOriginal_insurance_file_cnt AND risk_cnt = @nRisk_cnt

	-- Search behind also in the ladder as actual editing might have happened in some other policy version
	IF @bIs_risk_edited = 0 
	BEGIN
		WHILE @nOriginal_insurance_file_cnt <> 0
		BEGIN
			SELECT @nOriginal_iFileCnt = 0
			SELECT @nOriginal_iFileCnt = original_linked_insurance_file_cnt FROM insurance_file_risk_link ifrl	
				INNER JOIN mta_insurance_file_link mifl ON mifl.original_linked_insurance_file_cnt = ifrl.insurance_file_cnt
					WHERE ifrl.risk_cnt = @nRisk_cnt 
							AND (mifl.new_linked_insurance_file_cnt = @nOriginal_insurance_file_cnt OR mifl.cancelled_linked_insurance_file_cnt = @nOriginal_insurance_file_cnt)
								AND mifl.sequence_number > 1
					
			IF @nOriginal_iFileCnt = 0
				BREAK;	
			ELSE
			BEGIN
				SELECT @nOriginal_insurance_file_cnt = @nOriginal_iFileCnt
				
				SELECT @bIs_risk_edited = ISNULL(is_risk_edited, 0), @sStatus = status_flag FROM insurance_file_risk_link ifrl	
					INNER JOIN mta_insurance_file_link mifl ON mifl.original_linked_insurance_file_cnt = ifrl.insurance_file_cnt
						WHERE ifrl.risk_cnt = @nRisk_cnt AND mifl.original_linked_insurance_file_cnt = @nOriginal_insurance_file_cnt	
						
				IF @bIs_risk_edited = 1
					BREAK;	
				ELSE
					CONTINUE;		
			END	
		END
	END

	SELECT @nOriginal_iFileCnt = 0
	SELECT @nOriginal_iFileCnt = ISNULL(original_linked_insurance_file_cnt, 0) 
		FROM mta_insurance_file_link 
		WHERE cancelled_linked_insurance_file_cnt = @nOriginal_insurance_file_cnt
	
	-- if original iFile is OOS cancelled then search one level up if risk was deleted
	IF @nOriginal_iFileCnt > 0
	BEGIN	
		Select @sStatus = ifrl.status_flag
			From insurance_file_risk_link ifrl
				Inner Join risk r On r.risk_cnt = ifrl.risk_cnt
			Where r.risk_folder_cnt = @nRisk_folder_cnt And ifrl.insurance_file_cnt = @nOriginal_iFileCnt
	END
	
	SELECT @bIs_risk_edited, @nOriginal_insurance_file_cnt, @sStatus
	
END  
