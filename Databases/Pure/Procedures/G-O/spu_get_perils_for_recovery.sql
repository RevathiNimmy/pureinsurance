SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_get_perils_for_recovery'
GO

CREATE PROCEDURE spu_get_perils_for_recovery  
    @claim_id int,  
    @is_salvage int  
AS  
  
BEGIN
	SELECT 
		Recovery.claim_peril_id, 
		Claim_Peril.Description,  
		SUM(Recovery.Initial_reserve) AS IR,  
		SUM(Recovery.received_to_date) AS RTD,  
		SUM(Recovery.revised_reserve) AS RR,  
		SUM(Recovery.Initial_reserve) - SUM(Recovery.received_to_date) + SUM(Recovery.revised_reserve) AS Currres  
	FROM Claim_Peril 
		INNER JOIN  Recovery ON  
		      Claim_Peril.claim_peril_id = Recovery.claim_peril_id 
		INNER JOIN  Recovery_type ON  
		     Recovery.recovery_type_id = Recovery_type.recovery_type_id 
	WHERE Claim_Peril.Claim_id = @claim_id 
	AND  Recovery_type.is_salvage = @is_salvage  
	GROUP BY Recovery.claim_peril_id, Claim_Peril.Description  

END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
