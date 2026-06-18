SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure spu_CLM_Get_NetOf_Claim_Peril
GO

CREATE PROCEDURE spu_CLM_Get_NetOf_Claim_Peril    
@claim_id INT
AS    

	SELECT claim_peril_id, cob.class_of_business_id, RTRIM(cob.code)
	FROM claim_peril cp WITH(NOLOCK)
	LEFT JOIN Peril_Type pt ON pt.peril_type_id = cp.Peril_type_id
	LEFT JOIN Class_Of_Business cob ON cob.class_of_business_id = pt.class_of_business_id
	WHERE cp.claim_id = @claim_id

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO