SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON
GO

EXECUTE DDLDropProcedure 'spu_SAM_MID_Vehicle_Get'
GO

CREATE PROCEDURE spu_SAM_MID_Vehicle_Get  		
	@mid_policy_id			INT,   
	@failures_only			TINYINT 
AS  
SELECT 
	mv.mid_vehicle_id,
	mv.mid_policy_id,
	mv.update_type,
	mv.registration,
	mv.is_foreign_registration,
	mv.is_trade_registration,
	mv.make,
	mv.model,
	mv.on_date,
	mv.off_date,
	mv.reject_reference,
	mv.reject_error_codes,
	ms.code
FROM mid_vehicle mv
LEFT OUTER JOIN mid_status ms ON mv.mid_status_id = ms.mid_status_id
WHERE mv.mid_policy_id = @mid_policy_id
