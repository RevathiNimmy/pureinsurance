SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON
GO

EXECUTE DDLDropProcedure 'spu_SAM_MID_File_Get'
GO

CREATE PROCEDURE spu_SAM_MID_File_Get  		
	@batch_id				INT,   
	@failures_only			TINYINT 
AS  
SELECT 
	mp.mid_policy_id,
	ms.code,
	mp.insurance_file_cnt,
	ifi.insurance_ref,
	mp.ppcc,
	mp.update_type,
	mp.batch_id,
	b.batch_ref,
	mp.reject_reference,
	mp.reject_error_codes,
	mp.ppcc_expected,
	ms.description
FROM mid_policy mp
INNER JOIN insurance_file ifi ON mp.insurance_file_cnt = ifi.insurance_file_cnt
INNER JOIN batch b ON mp.batch_id = b.batch_id
LEFT OUTER JOIN mid_status ms ON mp.mid_status_id = ms.mid_status_id
WHERE mp.batch_id = @batch_id
AND ((@failures_only = 0) OR (ms.code = 'ERROR') OR 
		EXISTS(SELECT * FROM mid_vehicle mv
		INNER JOIN mid_policy mp ON mv.mid_policy_id = mp.mid_policy_id
		INNER JOIN mid_status ms ON mv.mid_status_id = ms.mid_status_id
		WHERE ms.code = 'ERROR' AND mp.batch_id = @batch_id))