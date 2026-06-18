SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_GetAllTPDetails'
GO

CREATE PROCEDURE spu_GetAllTPDetails  
    @Peril_id int  
AS  
  
BEGIN
	SELECT  
		recovery_id,  
		Claim_peril_id,  
		recovery.recovery_type_id,  
		currency_id,  
		initial_reserve,  
		revised_reserve,  
		received_to_date,  
		revision_count  
	FROM recovery,recovery_type  
	WHERE Claim_Peril_id = @Peril_Id 
	AND recovery.recovery_type_id=recovery_type.recovery_type_id  
	AND recovery.recovery_type_id IN
		(
			SELECT recovery_type_id 
			FROM recovery_type 
			WHERE is_salvage = 0
		)  
END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
