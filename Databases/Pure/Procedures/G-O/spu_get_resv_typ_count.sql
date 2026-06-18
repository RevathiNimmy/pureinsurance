SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_get_resv_typ_count'
GO

CREATE PROCEDURE spu_get_resv_typ_count  
    @claim_id int  
AS  
BEGIN

	SELECT DISTINCT  
		Reserve.Reserve_type_id, 
		Reserve_type.Description  
	FROM claim_Peril 
		INNER JOIN  Reserve ON 
			claim_Peril.claim_Peril_id = Reserve.claim_Peril_id 
		INNER JOIN  Reserve_type ON  
			Reserve.Reserve_type_id = Reserve_type.Reserve_type_id  
	WHERE claim_Peril.Claim_id = @claim_id  

END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
