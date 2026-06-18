SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_Claim_Peril_Class_Of_Business'
GO

CREATE PROCEDURE spu_CLM_Get_Claim_Peril_Class_Of_Business

@peril_type_id int,
@claim_peril_id int

AS

BEGIN

	IF @peril_type_id <> 0  
	BEGIN    
        	SELECT cob.class_of_business_id,
		       cob.code 
		FROM peril_type per, class_of_business cob
	        WHERE per.peril_type_id = @peril_type_id
	        AND per.class_of_business_id = cob.class_of_business_id
        END
           
	ELSE   
	BEGIN
		SELECT cob.class_of_business_id, 
			cob.code        
		FROM    claim_peril cp,
	        	peril_type pt,
		        class_of_business cob
	        WHERE cp.claim_peril_id = @claim_peril_id
        	AND cp.peril_type_id = pt.peril_type_id
	        AND pt.class_of_business_id = cob.class_of_business_id
	END
END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
