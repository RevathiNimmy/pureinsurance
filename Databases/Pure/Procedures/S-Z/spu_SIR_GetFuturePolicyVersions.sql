SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_GetFuturePolicyVersions'
GO

CREATE PROCEDURE spu_SIR_GetFuturePolicyVersions 
	@ninsurance_file_cnt INT  
AS  
BEGIN  

DECLARE @prev_insurance_file_cnt INT

SELECT @prev_insurance_file_cnt = 0
SELECT @prev_insurance_file_cnt = mifl_prev.new_linked_insurance_file_cnt 
FROM mta_insurance_file_link mifl_prev
	Inner Join mta_insurance_file_link mifl_curr 
			ON mifl_curr.insurance_file_cnt = mifl_prev.insurance_file_cnt
				And mifl_curr.sequence_number - 1 = mifl_prev.sequence_number
	WHERE mifl_curr.new_linked_insurance_file_cnt = @ninsurance_file_cnt 

-- for resilience only select base version
IF @prev_insurance_file_cnt = 0	
	SELECT @prev_insurance_file_cnt = insurance_file_cnt
		FROM mta_insurance_file_link 
		WHERE new_linked_insurance_file_cnt = @ninsurance_file_cnt

SELECT mifl.insurance_file_cnt, 
		mifl.original_linked_insurance_file_cnt, 
		mifl.cancelled_linked_insurance_file_cnt, 
		ifi.cover_start_date,
		ifi.expiry_date,
		ifinew.risk_processed,
		ifi.insurance_folder_cnt
FROM mta_insurance_file_link mifl
	Inner Join insurance_file ifi ON ifi.Insurance_File_Cnt = mifl.cancelled_linked_insurance_file_cnt 
	Inner Join insurance_file ifinew ON ifinew.Insurance_File_Cnt = mifl.new_linked_insurance_file_cnt 
WHERE mifl.new_linked_insurance_file_cnt = @ninsurance_file_cnt  

END  
