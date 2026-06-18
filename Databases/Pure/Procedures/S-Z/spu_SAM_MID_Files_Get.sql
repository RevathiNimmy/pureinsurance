SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON
GO

EXECUTE DDLDropProcedure 'spu_SAM_MID_Files_Get'
GO

CREATE PROCEDURE spu_SAM_MID_Files_Get  	
	@start_date				DATETIME = NULL,
	@end_date				DATETIME = NULL,
	@batch_id				INT		 = NULL,
	@failures_only			TINYINT
AS

SELECT DISTINCT
	b.import_file_name, b.batch_id, b.batch_ref, b.created_date,
	MIN(CASE WHEN (mp.reject_error_codes  like '%E001%'
				OR mp.reject_error_codes  like '%E002%'
				OR mp.reject_error_codes  like '%E003%'
				OR mp.reject_error_codes  like '%E004%'
				OR mp.reject_error_codes  like '%E005%'
				OR mp.reject_error_codes  like '%E006%'
				OR mp.reject_error_codes  like '%E007%'
				OR mp.reject_error_codes  like '%E008%'
				--OR mp.reject_error_codes  like '%E086%'
				OR mp.reject_error_codes  like '%E010%'
				OR mp.reject_error_codes  like '%E012%'
				OR mp.reject_error_codes  like '%E013%'
				OR mp.reject_error_codes  like '%E036%'
				OR mp.reject_error_codes  like '%E037%'
				OR mp.reject_error_codes  like '%E044%'
				--OR mp.reject_error_codes  like '%E058%'
				--OR mp.reject_error_codes  like '%E059%'
				--OR mp.reject_error_codes  like '%E060%'
				--OR mp.reject_error_codes  like '%E061%'
				--OR mp.reject_error_codes  like '%E085%'
				)
	THEN
		'Rejected'
	ELSE
		ms.description
	END) AS description

FROM batch b
INNER JOIN mid_policy mp ON b.batch_id = mp.batch_id
INNER JOIN mid_status ms ON mp.mid_status_id = ms.mid_status_id
WHERE (@batch_id IS NULL OR b.batch_id = @batch_id)
--AND (b.created_date >= @start_date AND b.created_date <= @end_date)
AND ((@failures_only = 0) OR (ms.code = 'ERROR')
		OR EXISTS(SELECT * FROM mid_vehicle mv
		INNER JOIN mid_policy mp ON mv.mid_policy_id = mp.mid_policy_id
		INNER JOIN mid_status ms ON mv.mid_status_id = ms.mid_status_id
		WHERE ms.code = 'ERROR' AND mp.batch_id = b.batch_id))
GROUP BY b.import_file_name, b.batch_id, b.batch_ref, b.created_date

GO