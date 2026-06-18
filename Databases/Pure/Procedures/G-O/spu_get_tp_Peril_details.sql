SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_get_tp_Peril_details'
GO

CREATE PROCEDURE spu_get_tp_Peril_details  
    @Claim_id int  
AS  
  
BEGIN
	SELECT 
		Claim_Peril.Claim_Peril_id,
		Claim_Peril.Peril_type_id, 
		Peril_Type.description,
		Claim_Peril.Description  
	FROM Claim_Peril 
		INNER JOIN  Peril_Type ON  
			Claim_Peril.Peril_type_id = Peril_Type.peril_type_id  
	WHERE (Claim_Peril.Claim_id =@Claim_id )  

END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
